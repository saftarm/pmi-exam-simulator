using TestAPI.DTO.AnswerOption;
using TestAPI.Entities;

namespace TestAPI.DTO.Question
{
    public record SessionResponseValidationContext
    {
      public IEnumerable<Guid> SubmittedQuestionIds {get;init;} = [];
      public IEnumerable<Guid> SessionQuestionIds {get;init;}= [];
      public IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> SelectedOptionIdsByQuestionId {get;init;} = new Dictionary<Guid, IReadOnlyList<Guid>>();
      public IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> AllowedOptionIdsByQuestionId {get;init;} = new Dictionary<Guid, IReadOnlyList<Guid>>();
      public IReadOnlyDictionary<Guid, QuestionType> QuestionTypeByQuestionId {get;init;}= new Dictionary<Guid, QuestionType>();
    }
}
