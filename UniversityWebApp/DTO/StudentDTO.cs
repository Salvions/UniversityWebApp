using UniversityWebApp.DATA;

namespace UniversityWebApp.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public List<CourseTipeDTO>? CourseTipeDTOs { get; set; }
        public List<ExamResultDTO>? ExamResultDTOs { get; set; }
    }
}
