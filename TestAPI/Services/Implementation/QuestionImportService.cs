using ClosedXML.Excel;
using FluentValidation;
using TestAPI.DTO.ImportService;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
    public class QuestionImportService : IQuestionImportService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IDomainRepository _domainRepository;
        private readonly IValidator<QuestionImportRowDto> _questionImportRowValidator;

        public QuestionImportService(
            IQuestionRepository questionRepository,
            IDomainRepository domainRepository,
            IValidator<QuestionImportRowDto> questionImportRowValidator)
        {
            _questionRepository = questionRepository;
            _domainRepository = domainRepository;
            _questionImportRowValidator = questionImportRowValidator;
        }

        private const int ColTitle = 1;
        private const int ColExplanation = 2;
        private const int ColDomainName = 3;
        private const int ColQuestionType = 4;

        private const int FirstOptionCol = 4;

        private const int MaxOptions = 4;
        private const int DataStartsRow = 4;

        public async Task<QuestionImportResultDto> ImportFromExcelAsync(IFormFile file, CancellationToken ct)
        {
            string extension = Path.GetExtension(file.FileName);
            var errors = new List<ImportRowErrorDto>();

            List<QuestionImportRowDto> rows;

            var stream = file.OpenReadStream();

            rows = ParseRows(stream, errors, _questionImportRowValidator, ct);


            var questions = new List<Question>();

            foreach (var questionRow in rows)
            {
                var domainId = await _domainRepository.GetIdByTitleAsync(questionRow.DomainName, ct);
                var question = new Question
                {
                    Title = questionRow.Title,
                    DomainId = domainId,
                    QuestionType = questionRow.QuestionType switch
                    {
                        "SingleChoice" => QuestionType.SingleChoice,
                        "MultipleChoice" => QuestionType.MultipleChoice,
                        "TrueFalse" => QuestionType.TrueFalse,
                        _ => throw new ArgumentException($"Unknown question type: {questionRow.QuestionType}")
                    },
                    Explanation = questionRow.Explanation,
                    AnswerOptions = questionRow.AnswerOptions.Select(o => new AnswerOption
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect == "TRUE" ? true : false

                    }).ToList()


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

            return result;

        }

        private static List<QuestionImportRowDto> ParseRows(Stream stream, List<ImportRowErrorDto> errors, IValidator<QuestionImportRowDto> validator, CancellationToken ct)
        {
            using var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheet("Questions");

            if (worksheet == null)
            {
                errors.Add(new ImportRowErrorDto { Row = 0, Reason = "Sheet named 'Questions' not found." });
            }
            var startRow = 3;
            var lastRow = worksheet.LastRowUsed().RowNumber();

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
