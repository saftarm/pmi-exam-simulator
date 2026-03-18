

using System.ComponentModel.DataAnnotations;

namespace TestAPI.DTO
{
    public class CompileExamDto
    {

        public int ExamId {get;set;}
        
        public List<int> QuestionIds { get; set; } = new List<int >();
       
    } 
}

