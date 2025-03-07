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
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Get student {id} avarege error");
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