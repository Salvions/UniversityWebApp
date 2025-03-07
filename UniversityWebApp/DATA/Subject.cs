namespace UniversityWebApp.DATA
{
    public class Subject
    {
        public int Id { get; set; }
        public required string? Title { get; set; }
        public List<CourseTipe>? CourseTipes { get; set; }
    }
}
