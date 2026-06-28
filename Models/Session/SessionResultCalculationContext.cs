using TestAPI.DTO.AnswerOption;
using TestAPI.Entities;

namespace TestAPI.Models.Session
{
    public record SessionResultCalculationContext
    {
      public Guid SessionId {get; init;}
      public int TotalQuestions {get;init;}
      public IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> SubmittedOptionsIdsByQuestionId {get;init;} = new Dictionary<Guid, IReadOnlyList<Guid>>();
      public IReadOnlyDictionary<Guid, QuestionType> QuestionTypeByQuestionId {get;init;}= new Dictionary<Guid, QuestionType>();
      public IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> CorrectOptionIdsByQuestionId {get;init;} = new Dictionary<Guid, IReadOnlyList<Guid>>();
      public IReadOnlyDictionary<Guid, Guid> DomainIdByQuestionId {get; init;} = new Dictionary<Guid, Guid>();
      
    }
}
