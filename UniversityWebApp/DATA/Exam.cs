using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebApp.DATA
{
    [PrimaryKey("StudentId", "CourseTipeId")]
    public class Exam
    {
        public int StudentId { get; set; }
        public int CourseTipeId { get; set; }
        public int Grade { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        [ForeignKey("CourseTipeId")]
        public CourseTipe? CourseTipe { get; set; }
    }
}
