using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityWebApp.DATA;
using UniversityWebApp.DTO;

namespace UniversityWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        public readonly Mapper _mapper;
        public readonly ILogger<TeacherController> _logger;
        public readonly UniversityDbContext _ctx;

        public TeacherController(Mapper mapper, ILogger<TeacherController> logger, UniversityDbContext ctx)
        {
            _mapper = mapper;
            _logger = logger;
            _ctx = ctx;
        }

        #region GET REQUESTS
        [HttpGet]
        public IActionResult GetAll()
        {
            var teachers = _ctx.Teachers.ToList();
            _logger.LogInformation("GetAll teachers");
            return Ok(teachers.ConvertAll(_mapper.TeacherToTeacherDTO));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            try
            {
                var teacher = _ctx.Teachers.Find(id);
                if (teacher == null)
                {
                    _logger.LogError($"Teacher {id} not found");
                    return BadRequest($"Teacher {id} not found");
                }
                _logger.LogInformation($"Get teacher {id}");
                return Ok(_mapper.TeacherToTeacherDTO(teacher));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Teacher error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/FutureExams")]
        public IActionResult GetFutureExams(int id)
        {
            try
            {
                var teacher = _ctx.Teachers.Include(x => x.Exams)
                    .SingleOrDefault(x => x.Id == id);
                if (teacher == null)
                {
                    _logger.LogError($"Teacher {id} not found");
                    return BadRequest($"Teacher {id} not found");
                }
                TeacherDTO teacherDTO = _mapper.TeacherToTeacherDTO(teacher);
                _logger.LogInformation($"Get teacher {id} && FutureExams");
                teacherDTO.ExamDTOs = teacher.Exams
                    .Where(x => x.Date > DateTime.Now)
                    .Select(x => _mapper.ExamToExamDTO(x))
                    .ToList();
                return Ok(teacherDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get FutureExams error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
