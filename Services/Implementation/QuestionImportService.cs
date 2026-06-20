using ClosedXML.Excel;
using FluentValidation;
using TestAPI.DTO.ImportService;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
  public class QuestionImportService : IQuestionImportService
  {
    private readonly ILogger<IQuestionImportService> _logger;
    private readonly IQuestionRepository _questionRepository;
    private readonly IDomainRepository _domainRepository;
    private readonly IValidator<QuestionImportRowDto> _questionImportRowValidator;

    public QuestionImportService(
        ILogger<IQuestionImportService> logger,
        IQuestionRepository questionRepository,
        IDomainRepository domainRepository,
        IValidator<QuestionImportRowDto> questionImportRowValidator)
    {
      _logger = logger;
      _questionRepository = questionRepository;
      _domainRepository = domainRepository;
      _questionImportRowValidator = questionImportRowValidator;
    }

    private const int ColTitle = 1;
    private const int ColExplanation = 2;
    private const int ColDomainId = 3;
    private const int ColQuestionType = 4;

    public async Task<Result<QuestionImportResultDto>> ImportFromExcelAsync(IFormFile file, CancellationToken ct)
    {
      var errors = new List<ImportRowErrorDto>();
      var stream = file.OpenReadStream();
      var rows = ParseRows(stream, errors, _questionImportRowValidator);

      if (errors.Count > 0)
      {
        return Result<QuestionImportResultDto>.Success(new QuestionImportResultDto
        {
          Success = false,
          ImportedCount = 0,
          Errors = errors
        });
      }

      var questions = new List<Question>();

      foreach (var questionRow in rows)
      {
        var domain = await _domainRepository.GetByIdAsync(questionRow.DomainId);
        if (domain == null)
        {
          errors.Add(new ImportRowErrorDto
          {
            Row = questionRow.RowNumber,
            Reason = $"Domain not found for DomainId {questionRow.DomainId}"
          });
          continue;
        }

        var question = new Question(
            title: questionRow.Title,
            explanation: questionRow.Explanation!,
            domainId: questionRow.DomainId,
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
              domainId: questionRow.DomainId))]
        };

        questions.Add(question);
      }

      if (errors.Count > 0)
      {
        return Result<QuestionImportResultDto>.Success(new QuestionImportResultDto
        {
          Success = false,
          ImportedCount = 0,
          Errors = errors
        });
      }

      await _questionRepository.AddRangeAsync(questions);

      if (_logger.IsEnabled(LogLevel.Information))
      {
        _logger.LogInformation("Imported {Count} questions from Excel", questions.Count);
      }

      return Result<QuestionImportResultDto>.Success(new QuestionImportResultDto
      {
        Success = true,
        ImportedCount = questions.Count,
        Errors = errors
      });
    }

    private static List<QuestionImportRowDto> ParseRows(
        Stream stream,
        List<ImportRowErrorDto> errors,
        IValidator<QuestionImportRowDto> validator)
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

        var domainIdRaw = worksheet.Cell(rowNum, ColDomainId).GetString().Trim();
        if (!Guid.TryParse(domainIdRaw, out var domainId))
        {
          errors.Add(new ImportRowErrorDto
          {
            Row = rowNum,
            Reason = $"Invalid DomainId at column {ColDomainId}: '{domainIdRaw}'"
          });
          continue;
        }

        var question = new QuestionImportRowDto
        {
          RowNumber = rowNum,
          Title = worksheet.Cell(rowNum, ColTitle).GetString().Trim(),
          Explanation = worksheet.Cell(rowNum, ColExplanation).GetString().Trim(),
          DomainId = domainId,
          QuestionType = worksheet.Cell(rowNum, ColQuestionType).GetString().Trim(),
          AnswerOptions = answerOptions
        };

        var context = new ValidationContext<QuestionImportRowDto>(question);
        context.RootContextData["ExcelMetadata"] = new ExcelRowContext(
            rowNum,
            ColTitle,
            ColExplanation,
            ColDomainId,
            ColQuestionType);

        var result = validator.Validate(context);

        if (!result.IsValid)
        {
          foreach (var failure in result.Errors)
          {
            errors.Add(new ImportRowErrorDto
            {
              Row = rowNum,
              Reason = failure.ErrorMessage
            });
          }
          continue;
        }

        questions.Add(question);
      }

      return questions;
    }
  }
}
