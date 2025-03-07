using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebApp.DATA;

namespace UniversityWebApp.DTO
{
    public class ExamDTO
    {
        public int Id { get; set; }
        public int CourseTipeId { get; set; }
        public required string Location { get; set; }
        public required DateTime Date { get; set; }
        public CourseTipeDTO? CourseTipeDTO { get; set; }
        public List<ExamResultDTO> examResultDTOs { get; set; }
    }
}
