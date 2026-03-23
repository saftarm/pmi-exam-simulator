namespace TestAPI.DTO
{
    public class CreateExamsDto
    {

        public ICollection<CreateExamDto> CreateExamDtos {get;set;} = new List<CreateExamDto>();

    }
}
