namespace UniversityWebApp.DTO
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public required string? Name { get; set; }
        public required string Surname { get; set; }
        public List<ExamDTO>? ExamDTOs { get; set; }
    }
}