﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebApp.DATA
{
    public class Exam
    {
        public int StudentId { get; set; }
        public int CourseTipeId { get; set; }
        public required string Location { get; set; }
        public required DateTime Date { get; set; }

        [ForeignKey("CourseTipeId")]
        public CourseTipe? CourseTipe { get; set; }
    }
}
