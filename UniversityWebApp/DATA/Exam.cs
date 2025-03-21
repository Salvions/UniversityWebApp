﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebApp.DATA
{
    public class Exam
    {
        public int Id { get; set; }
        public int CourseTipeId { get; set; }
        public int? TeacherId { get; set; }
        public required string Location { get; set; }
        public required DateTime Date { get; set; }

        [ForeignKey("CourseTipeId")]
        public CourseTipe? CourseTipe { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }
        public List<ExamResult>? ExamResults { get; set; }

    }
}
