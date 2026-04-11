using FluentValidation;
using TestAPI.DTO.ImportService;
using TestAPI.Models;

namespace TestAPI.Validation
{

    public class QuestionValidator : AbstractValidator<QuestionImportRowDto>
    {
        public QuestionValidator()
        {
            RuleFor(q => q.Title).Custom((title, context) =>
            {
                if (string.IsNullOrEmpty(title))
                {
                    if (context.RootContextData.TryGetValue("ExcelMetadata", out var raw) && raw is ExcelRowContext metadata)
                    {
                        context.AddFailure("Title", $"Title must not be empty, Row: {metadata.RowNum}, Column:{metadata.ColTitle}");
                    }

                }
            });

            RuleFor(q => q.Explanation).Custom((explanation, context) =>
            {
                if (string.IsNullOrEmpty(explanation))
                {
                    if (context.RootContextData.TryGetValue("ExcelMetadata", out var raw) && raw is ExcelRowContext metadata)
                    {
                        context.AddFailure("Explanation", $"Explanation must not be empty, Row: {metadata.RowNum}, Column:{metadata.ColExplanation}");
                    }
                }
            });

            RuleFor(q => q.DomainName).Custom((domain, context) =>
            {
                if (string.IsNullOrEmpty(domain))
                {
                    if (context.RootContextData.TryGetValue("ExcelMetadata", out var raw) && raw is ExcelRowContext metadata)
                    {
                        context.AddFailure("Domain", $"Dpmain must not be empty, Row: {metadata.RowNum}, Column:{metadata.ColDomain}");
                    }

                }
            });

            RuleFor(q => q.QuestionType).Custom((questionType, context) =>
            {
                if (string.IsNullOrEmpty(questionType))
                {
                    if (context.RootContextData.TryGetValue("ExcelMetadata", out var raw) && raw is ExcelRowContext metadata)
                    {
                        context.AddFailure("QuestionType", $"Question Type must not be empty, Row: {metadata.RowNum}, Column:{metadata.ColQuestionType}");
                    }

                }
            });

            RuleFor(q => q.QuestionType)
                .Must(t => new[] { "SingleChoice", "MultipleChoice", "TrueFalse" }.Contains(t))
                .WithMessage("Invalid Question Type");
        }



    }
}