

using System.ComponentModel.DataAnnotations;

namespace TestAPI.DTO
{
    public class CompileExamDto
    {

        public Guid ExamId {get;set;}
        
        public List<Guid> QuestionIds { get; set; } = new List<Guid>();
       
    } 
}

