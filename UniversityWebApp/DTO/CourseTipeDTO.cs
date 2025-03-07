using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebApp.DATA;

namespace UniversityWebApp.DTO
{
    public class CourseTipeDTO
    {
        public int Id { get; set; }
        public int Credits { get; set; }
        public required string Title { get; set; }
        public SubjectDTO? SubjectDTO { get; set; }
        public CourseDTO? CourseDTO { get; set; }
        public List<ExamDTO>? ExamDTOs { get; set; }
    }
}
