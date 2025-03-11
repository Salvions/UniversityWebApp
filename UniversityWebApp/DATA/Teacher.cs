namespace UniversityWebApp.DATA
{
    public class Teacher
    {
        public int Id { get; set; }
        public required string? Name { get; set; }
        public required string Surname { get; set; }

        public List<Exam>? Exam { get; set; }
    }
}
