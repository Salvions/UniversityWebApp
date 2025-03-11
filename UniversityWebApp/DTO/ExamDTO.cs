using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebApp.DATA;

namespace UniversityWebApp.DTO
{
    public class ExamDTO
    {
        public int Id { get; set; }
        public int CourseTipeId { get; set; }
        public int? TeacherId { get; set; }
        public required string Location { get; set; }
        public required DateTime Date { get; set; }
        public double? AverageOrGrade { get; set; }
        public TeacherDTO? TeacherDTO { get; set; }
        public CourseTipeDTO? CourseTipeDTO { get; set; }
        public List<StudentDTO>? Students { get; set; }
        public List<ExamResultDTO>? ExamResultDTOs { get; set; }
    }
}
