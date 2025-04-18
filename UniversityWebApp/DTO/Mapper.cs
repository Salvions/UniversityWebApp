﻿using Microsoft.Identity.Client;
using UniversityWebApp.DATA;
using UniversityWebApp.DTO;

namespace UniversityWebApp.DTO
{
    public class Mapper
    {
        #region COURSE MAPPER
        public Course CourseDTOtoCourse(CourseDTO courseDTO)
        {
            return new Course
            {
                Id = courseDTO.Id,
                Title = courseDTO.Title,
                Tipology = courseDTO.Tipology,
                StartDate = courseDTO.StartDate,
            };
        }
        public CourseDTO CourseToCourseDTO(Course course)
        {
            return new CourseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Tipology = course.Tipology,
                StartDate = (DateTime)course.StartDate
            };
        }
        #endregion

        #region SUBJECT MAPPER
        public Subject SubjectToCourseDTO(SubjectDTO subjectDTO)
        {
            return new Subject
            {
                Id = subjectDTO.Id,
                Title = subjectDTO.Title,
            };
        }

        public SubjectDTO SubjectToCourseDTO(Subject subject)
        {
            return new SubjectDTO
            {
                Id = subject.Id,
                Title = subject.Title,
            };
        }
        #endregion

        #region STUDENT MAPPER

        public Student StudentDTOtoStudent(StudentDTO studentDTO)
        {
            return new Student
            {
                Id = studentDTO.Id,
                Name = studentDTO.Name,
                Surname = studentDTO.Surname,
            };
        }

        public StudentDTO StudentToStudentDTO(Student student)
        {
            return new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
            };
        }

        #endregion

        #region EXAM MAPPER
        public Exam ExamDTOtoExam(ExamDTO examDTO)
        {
            return new Exam
            {
                Id = examDTO.Id,
                Date = examDTO.Date,
                Location = examDTO.Location,
                CourseTipeId = examDTO.CourseTipeId,
            };
        }

        public ExamDTO ExamToExamDTO(Exam exam)
        {
            return new ExamDTO
            {
                Id = exam.Id,
                Date = exam.Date,
                Location = exam.Location,
                CourseTipeId = exam.CourseTipeId,
            };
        }
        #endregion

        #region EXAMRESULT MAPPER
        public ExamResult ExamResultDTOtoExamResult(ExamResultDTO examResultDTO)
        {
            return new ExamResult
            {
                
                StudentId = examResultDTO.StudentId,
                ExamId = examResultDTO.ExamId,
                ResultId = ((int)examResultDTO.Result)
            };
        }

        public ExamResultDTO ExamResultToExamResultDTO(ExamResult examResult)
        {
            return new ExamResultDTO
            {
                StudentId = examResult.StudentId,
                ExamId = examResult.ExamId,
                Result = (Result)examResult.ResultId
            };
        }
        #endregion

        #region COURSETIPE MAPPER

        public CourseTipe CourseTipeDTOtoCourseTipe(CourseTipeDTO courseTipeDTO)
        {
            return new CourseTipe
            {
                Id = courseTipeDTO.Id,
                Credits = courseTipeDTO.Credits,
                Title = courseTipeDTO.Title
            };
        }

        public CourseTipeDTO CourseTipeToCourseTipeDTO(CourseTipe courseTipe)
        {
            return new CourseTipeDTO
            {
                Id = courseTipe.Id,
                Credits = courseTipe.Credits,
                Title = courseTipe.Title
            };
        }
        #endregion

        #region REGISTRED MAPPER

        public Registred RegistredDTOtoRegistred(RegistredDTO registredDTO)
        {
            return new Registred
            {
                StudentId = registredDTO.StudentId,
                CourseId = registredDTO.CourseId,
                RegistredDate = registredDTO.RegistredDate
            };
        }

        public RegistredDTO RegistredDTOtoRegistred(Registred registred)
        {
            return new RegistredDTO
            {
                StudentId = registred.StudentId,
                CourseId = registred.CourseId,
                RegistredDate = registred.RegistredDate
            };
        }

        #endregion

        #region SUBJECT MAPPER

        public Subject SubjectDTOtoSubject(SubjectDTO subjectDTO)
        {
            return new Subject
            {
                Id = subjectDTO.Id,
                Title = subjectDTO.Title
            };
        }

        public SubjectDTO SubjectToSubjectDTO(Subject subject)
        {
            return new SubjectDTO
            {
                Id = subject.Id,
                Title = subject.Title
            };
        }

        #endregion

        #region TEACHER MAPPER
        public TeacherDTO TeacherToTeacherDTO(Teacher input)
        {
            return new TeacherDTO
            {
                Id = input.Id,
                Name = input.Name,
                Surname = input.Surname
            };
        }

        public Teacher TeacherDTOtoTeacher(TeacherDTO input)
        {
            return new Teacher
            {
                Id = input.Id,
                Name = input.Name,
                Surname = input.Surname
            };
        }
        #endregion
    }
}
