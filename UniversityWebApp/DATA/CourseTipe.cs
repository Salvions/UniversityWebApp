using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebApp.DATA
{
    public class CourseTipe
    {
        public int Id { get; set; }
        public int Credits { get; set; }
        public int SubjectId { get; set; }
        public int CourseId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public List<Exam>? Exams { get; set; }
    }
}
