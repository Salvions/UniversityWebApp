using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace UniversityWebApp.DATA
{
    [PrimaryKey("StudentId", "ExamId")]
    public class ExamResult
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int Grade { get; set; }
        public enum Result { Passed, Failed, Rejected }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        [ForeignKey("ExamId")]
        public Exam? Exam { get; set; }
    }
}
