using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using TestAPI.DTO.ImportService;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace TestAPI.Services.Implementation
{
  public class QuestionImportService : IQuestionImportService
  {
    private readonly ILogger<IQuestionImportService> _logger;
    private readonly IExamRepository _examRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IDomainRepository _domainRepository;
    private readonly IValidator<QuestionImportRowDto> _questionImportRowValidator;
    public QuestionImportService(
        ILogger<IQuestionImportService> logger,
        IExamRepository examRepository,
        IQuestionRepository questionRepository,
        IDomainRepository domainRepository,
        IValidator<QuestionImportRowDto> questionImportRowValidator)
    {
      _logger = logger;
      _examRepository = examRepository;
      _questionRepository = questionRepository;
      _domainRepository = domainRepository;
      _questionImportRowValidator = questionImportRowValidator;
    }

    private const int ColTitle = 1;
    private const int ColExplanation = 2;
    private const int ColDomainName = 3;
    private const int ColQuestionType = 4;

    public async Task<Result<QuestionImportResultDto>> ImportFromExcelAsync(Guid examId, IFormFile file, CancellationToken ct)
    {
      var errors = new List<ImportRowErrorDto>();

      List<QuestionImportRowDto> rows;

      var stream = file.OpenReadStream();

      rows = ParseRows(stream, errors, _questionImportRowValidator);

      var questions = new List<Question>();
      var domainIdsWithTitles = await _domainRepository.GetDomainIdsWithTitlesByExamId(examId);

      if(!domainIdsWithTitles.Any()) {
        _logger.LogInformation("{domainIdsWithTitles} is empty", nameof(domainIdsWithTitles));
      }

      foreach (var (domainId, title) in domainIdsWithTitles)
      {
        _logger.LogInformation("DomainId: {DomainId}, Title: {Title}", domainId, title);

        Console.WriteLine($"DomainId: {domainId}, Title: {title}");
      }

      foreach (var questionRow in rows)
      {
        var match = domainIdsWithTitles.FirstOrDefault(d => string.Equals(d.Value, questionRow.DomainName, StringComparison.OrdinalIgnoreCase));

        if (match.Key == Guid.Empty)
        {
          _logger.LogInformation("No matching domainId found for {domainName}", questionRow.DomainName);
          errors.Add(new ImportRowErrorDto
          {
            Row = 1,
            Reason = $"No matching domainId found for {questionRow.DomainName}"
          });
        }


        var domainId = domainIdsWithTitles.FirstOrDefault(d => d.Value == questionRow.DomainName!).Key;
        var question = new Question(
            title: questionRow.Title,
            explanation: questionRow.Explanation!,
            domainId: domainId,
            questionType: questionRow.QuestionType switch
            {
              "SingleChoice" => QuestionType.SingleChoice,
              "MultipleChoice" => QuestionType.MultipleChoice,
              "TrueFalse" => QuestionType.TrueFalse,
              _ => throw new ArgumentException($"Unknown question type: {questionRow.QuestionType}")
            })
        {
          AnswerOptions = [.. questionRow.AnswerOptions.Select(o => new AnswerOption(
                          text: o.Text,
                          isCorrect: o.IsCorrect == "TRUE",
                          domainId: domainId)
                    {
                    })]
        };

        questions.Add(question);

      }
      await _questionRepository.AddRangeAsync(questions);

      var result = new QuestionImportResultDto
      {
        Success = true,
        ImportedCount = questions.Count,
        Errors = errors
      };

      return Result<QuestionImportResultDto>.Success(result);


    }


    private static List<QuestionImportRowDto> ParseRows(Stream stream, List<ImportRowErrorDto> errors, IValidator<QuestionImportRowDto> validator)
    {
      using var workbook = new XLWorkbook(stream);

      var worksheet = workbook.Worksheet("Questions");

      if (worksheet == null)
      {
        errors.Add(new ImportRowErrorDto { Row = 0, Reason = "Sheet named 'Questions' not found." });
        return [];
      }
      var startRow = 3;
      var lastRowUsed = worksheet.LastRowUsed();
      if (lastRowUsed == null)
      {
        return [];
      }

      var lastRow = lastRowUsed.RowNumber();
      var questions = new List<QuestionImportRowDto>();

      for (int rowNum = startRow; rowNum <= lastRow; rowNum++)
      {
        var answerOptions = new List<AnswerOptionImportDto>();
        for (int i = 0; i <= 4; i++)
        {
          int col = 5 + (i * 2);
          var text = worksheet.Cell(rowNum, col).GetString().Trim();
          var isCorrect = worksheet.Cell(rowNum, col + 1).GetString().Trim();
          if (!string.IsNullOrEmpty(text))
          {
            answerOptions.Add(new AnswerOptionImportDto { Text = text, IsCorrect = isCorrect });
          }
        }

        var question = new QuestionImportRowDto
        {
          Title = worksheet.Cell(rowNum, ColTitle).GetString().Trim(),
          Explanation = worksheet.Cell(rowNum, ColExplanation).GetString().Trim(),
          DomainName = worksheet.Cell(rowNum, ColDomainName).GetString().Trim(),
          QuestionType = worksheet.Cell(rowNum, ColQuestionType).GetString().Trim(),
          AnswerOptions = answerOptions
        };

        var context = new ValidationContext<QuestionImportRowDto>(question);
        context.RootContextData["ExcelMetadata"] = new ExcelRowContext(
            rowNum,
            ColTitle,
            ColExplanation,
            ColDomainName,
            ColQuestionType);

        var result = validator.Validate(context);

        if (!result.IsValid)
        {
          throw new ValidationException(result.Errors);
        }

        questions.Add(question);

      }
      return questions;
    }


  }
}
