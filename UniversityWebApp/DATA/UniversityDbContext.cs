using Microsoft.EntityFrameworkCore;

namespace UniversityWebApp.DATA
{
    public class UniversityDbContext: DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ResultType>().HasData(new List<ResultType>
            {
                new ResultType {Id = 1, Title = "Passed"},
                new ResultType {Id = 2, Title = "Failed"},
                new ResultType {Id = 3, Title = "Rejected"}
            });
        }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTipe> CourseTipes { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Registred> Registreds { get; set; }
    }
}