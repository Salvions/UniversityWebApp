using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebApp.DATA
{
    [PrimaryKey("StudentId", "CourseId")]
    public class Registred
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime RegistredDate { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}
