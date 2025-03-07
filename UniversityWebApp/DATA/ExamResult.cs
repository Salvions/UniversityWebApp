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
        public int ResultId { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        [ForeignKey("ExamId")]
        public Exam? Exam { get; set; }
        [ForeignKey("ResultId")]
        public ResultType? Result { get; set; }
    }

    public class ResultType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ExamResult>? ExamResults { get; set; }
    }
}
