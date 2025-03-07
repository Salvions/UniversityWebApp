using UniversityWebApp.DATA;

namespace UniversityWebApp.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public required string? Title { get; set; }
        public List<CourseTipeDTO>? CourseTipeDTOs { get; set; }
    }
}
