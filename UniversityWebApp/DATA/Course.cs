namespace UniversityWebApp.DATA
{
    public class Course
    {
        public int Id { get; set; }
        public required string? Title { get; set; }
        public required string? Tipology { get; set; }
        public List<CourseTipe>? CourseTipes { get; set; }
        public List<Registred>? Registreds { get; set; }
    }
}
