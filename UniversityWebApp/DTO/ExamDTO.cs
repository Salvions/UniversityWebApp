using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebApp.DATA;

namespace UniversityWebApp.DTO
{
    public class ExamDTO
    {
        public int StudentId { get; set; }
        public int CourseTipeId { get; set; }
        public int Grade { get; set; }
        public StudentDTO? StudentDTO { get; set; }
        public CourseTipeDTO? CourseTipeDTO { get; set; }
    }
}
