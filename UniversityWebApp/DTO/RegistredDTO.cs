namespace UniversityWebApp.DTO
{
    public class RegistredDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime RegistredDate { get; set; }
        public StudentDTO? Student { get; set; }
        public CourseDTO? Course { get; set; }
    }
}
