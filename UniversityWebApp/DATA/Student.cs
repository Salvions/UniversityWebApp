﻿namespace UniversityWebApp.DATA
{
    public class Student
    {
        public int Id { get; set; }
        public required string?  Name { get; set; }
        public required string  Surname { get; set; }
        public List<ExamResult>? ExamResults { get; set; }
        public List<Registred>? Registreds { get; set; }
    }
}
