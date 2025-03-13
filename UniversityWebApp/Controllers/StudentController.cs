using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityWebApp.DATA;
using UniversityWebApp.DTO;

namespace University.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UniversityDbContext _ctx;
        private readonly Mapper _mapper;
        private readonly ILogger<StudentController> _logger;

        public StudentController(UniversityDbContext ctx, Mapper mapper, ILogger<StudentController> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        #region GET REQUESTS
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _ctx.Students.ToList();
            _logger.LogInformation("GetAll students");
            return Ok(students.ConvertAll(_mapper.StudentToStudentDTO));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var student = _ctx.Students.Find(id);
                if (student == null)
                {
                    _logger.LogInformation($"Get student {id} not found");
                    return NotFound();
                }
                _logger.LogInformation($"Get student {id}");
                return Ok(_mapper.StudentToStudentDTO(student));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Get student {id} error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/exams")]
        public IActionResult GetExams(int id)
        {
            try
            {
                var student = _ctx.Students.Include(w => w.ExamResults)
                    .SingleOrDefault(w => w.Id == id);
                if (student == null)
                {
                    return NotFound();
                }
                StudentDTO dto = _mapper.StudentToStudentDTO(student);
                dto.ExamResultDTOs = student.ExamResults.ConvertAll(_mapper.ExamResultToExamResultDTO);
                return Ok(dto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Get student {id} exams error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Avarege")]
        public IActionResult GetAvarege(int id)
        {
            try
            {
                var student = _ctx.Students.Include(w => w.ExamResults)
                    .ThenInclude(w => w.Exam)
                    .ThenInclude(w => w.CourseTipe)
                    .SingleOrDefault(w => w.Id == id);
                if (student == null)
                {
                    _logger.LogError(id, $"Get student {id} avarege not found");
                    return NotFound();
                }
                var Gsum = student.ExamResults.Sum(x => x.Grade * x.Exam.CourseTipe.Credits);
                var Csum= student.ExamResults.Sum(x => x.Exam.CourseTipe.Credits);
                var avarege = Gsum / Csum;
                StudentDTO sDTO= _mapper.StudentToStudentDTO(student);
                sDTO.Average = avarege;
                _logger.LogInformation($"Get student {id} && avarege");
                return Ok(sDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Get student {id} && avarege error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion

        #region POST REQUESTS
        [HttpPost]
        public IActionResult Post([FromBody] StudentDTO studentDTO)
        {
            try
            {
                var student = _mapper.StudentDTOtoStudent(studentDTO);
                if (_ctx.Students.Any(w => w.Id == student.Id))
                {
                    _logger.LogInformation("Post student conflict");
                    return Conflict();
                }
                _ctx.Students.Add(student);
                _ctx.SaveChanges();
                _logger.LogInformation("Post student");
                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Post student error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("RegisterExams")]
        public IActionResult PostExam([FromBody] ExamResultDTO examResultDTO)
        {
            var exr=_ctx.ExamResults.ToList();
            var st=_ctx.Students.Include(x=>x.Registreds)
                .ThenInclude(x => x.Course)
                .SingleOrDefault(x=>x.Id == examResultDTO.StudentId);
            var ex=_ctx.Exams.Include(x => x.CourseTipe)
                .ToList();

            try
            {
                var examResult = _mapper.ExamResultDTOtoExamResult(examResultDTO);
                if (exr.Any(w => w.StudentId == examResult.StudentId && w.ExamId == examResult.ExamId))
                {
                    _logger.LogInformation("Post student register exams conflict");
                    return Conflict();
                }
                foreach(var e in ex)
                {
                    if (e.Id == examResult.ExamId)
                    {
                        if(st.Registreds.SingleOrDefault(x => x.CourseId == e.CourseTipe.CourseId) == null)
                        {
                            _logger.LogInformation("Post student register exams not registred");
                            return BadRequest();
                        }
                        else
                        {
                            examResult.Grade = -1;
                            _ctx.ExamResults.Add(examResult);
                            _ctx.SaveChanges();
                            _logger.LogInformation("Post student register exams");
                            return Created();
                        }
                    } 
                }
                return BadRequest();
            }
            catch
            {
                _logger.LogError("Post student register exams error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("RegisterExams")]
        public IActionResult RgisterExams([FromBody] List<ExamResultDTO> ExamResultDTOs)
        {
            var students = _ctx.Students.Include(x => x.Registreds)
                .ThenInclude(x => x.Course)
                .ToList();
            var exams = _ctx.Exams.Include(x => x.CourseTipe).ToList();
            var exr = _ctx.ExamResults.ToList();
            foreach (var exd in ExamResultDTOs)
            {
                try
                {
                    if (exr.Any(x => x.ExamId == exd.ExamId && x.StudentId == exd.StudentId))
                    {
                        _logger.LogInformation("Post student register all exams conflict");
                        break;
                    }
                    var st = students.SingleOrDefault(x => x.Id == exd.StudentId);
                    if (st == null)
                    {
                        _logger.LogInformation("Post student register all exams not found");
                        break;
                    }
                    var ex = exams.SingleOrDefault(x => x.Id == exd.ExamId);
                    if (ex == null)
                    {
                        _logger.LogInformation("Post student register all exams not found");
                        break;
                    }
                    if (st.Registreds.SingleOrDefault(x => x.CourseId == ex.CourseTipe.CourseId) == null)
                    {
                        _logger.LogInformation("Post student register all exams not registred");
                        break;
                    }
                    else
                    {
                        var examResult = _mapper.ExamResultDTOtoExamResult(exd);
                        examResult.Grade = -1;
                        _ctx.ExamResults.Add(examResult);
                        _ctx.SaveChanges();
                        _logger.LogInformation("Post student register all exams");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Post student register all exams error");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }   
            }
            return Created();
        }

        #endregion

        #region DELETE REQUESTS
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var student = _ctx.Students.Find(id);
                if (student == null)
                {
                    _logger.LogInformation($"Delete student {id} not found");
                    return NotFound();
                }
                _ctx.Students.Remove(student);
                _ctx.SaveChanges();
                _logger.LogInformation($"Delete student {id}");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Delete student {id} error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region PUT REQUESTS
        [HttpPut("{idStudent}/{idCourse}")]
        public IActionResult Put(int idStudent, int idCourse)
        {
            try
            {
                var reg = _ctx.Registreds.Find(idStudent, idCourse);
                if (reg != null)
                {
                    return Conflict();
                }
                var cor = _ctx.Courses.Find(idCourse);
                if (cor.CourseTipes.Sum(x=>x.Credits)<130)
                {
                    return BadRequest();
                }
                reg = new Registred
                {
                    StudentId = idStudent,
                    CourseId = idCourse,
                    RegistredDate = DateTime.Now
                };
                _ctx.Registreds.Add(reg);
                _ctx.SaveChanges();
                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Put student error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}