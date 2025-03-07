namespace UniversityWebApp.DTO
{
    public class ExamResultDTO
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int Grade { get; set; }
        public enum Result { Passed, Failed, Rejected }
        public StudentDTO? StudentDTO { get; set; }
        public ExamDTO? ExamDTO { get; set; }
    }
}
