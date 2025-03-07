using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityWebApp.DATA;
using UniversityWebApp.DTO;

namespace UniversityWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        public readonly Mapper _mapper;
        public readonly ILogger<ExamsController> _logger;
        public readonly UniversityDbContext _ctx;
        public ExamsController(Mapper mapper, ILogger<ExamsController> logger, UniversityDbContext ctx)
        {
            _mapper = mapper;
            _logger = logger;
            _ctx = ctx;
        }

        #region GET REQUESTS

        [HttpGet]
        public IActionResult GetAll()
        {
            var exams = _ctx.Exams.ToList();
            _logger.LogInformation("GetAll exams");
            return Ok(exams.ConvertAll(_mapper.ExamToExamDTO));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var exam = _ctx.Exams.Find(id);
                if (exam == null)
                {
                    _logger.LogError($"Exam {id} not found");
                    return BadRequest($"Exam {id} not found");
                }
                _logger.LogInformation($"Get exam {id}");
                return Ok(_mapper.ExamToExamDTO(exam));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Exam error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Course")]
        public IActionResult GetCourse(int id) {
            try
            {
                var exam = _ctx.Exams.Include(x=>x.CourseTipe)
                    .SingleOrDefault(x => x.Id == id);
                if (exam == null)
                {
                    _logger.LogError($"Exam {id} not found");
                    return BadRequest($"Exam {id} not found");
                }
                ExamDTO examDTO = _mapper.ExamToExamDTO(exam);
                _logger.LogInformation($"Get exam {id}");
                examDTO.CourseTipeDTO = _mapper.CourseTipeToCourseTipeDTO(exam.CourseTipe);
                return Ok(examDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Exam error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Student")]
        public IActionResult GetStudent(int id)
        {
            try
            {
                var exam = _ctx.Exams.Include(x => x.ExamResults)
                    .ThenInclude(x => x.Student)
                    .SingleOrDefault(x => x.Id == id);
                if (exam == null)
                {
                    _logger.LogError($"Exam {id} not found");
                    return BadRequest($"Exam {id} not found");
                }
                ExamDTO examDTO = _mapper.ExamToExamDTO(exam);
                examDTO.Students = exam.ExamResults.Select(x => _mapper.StudentToStudentDTO(x.Student)).ToList();
                _logger.LogInformation($"Get exam {id}");
                return Ok(examDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Exam error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Result")]
        public IActionResult GetResult(int id)
        {
            try
            {
                var exam = _ctx.Exams.Include(x => x.ExamResults)
                    .SingleOrDefault(x => x.Id == id);
                if (exam == null)
                {
                    _logger.LogError($"Exam {id} not found");
                    return BadRequest($"Exam {id} not found");
                }
                ExamDTO examDTO = _mapper.ExamToExamDTO(exam);
                examDTO.ExamResultDTOs = exam.ExamResults.Select(x => _mapper.ExamResultToExamResultDTO(x)).ToList();                
                _logger.LogInformation($"Get exam {id}");
                return Ok(examDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Exam error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion

        #region POST REQUESTS

        [HttpPost]
        public IActionResult Post([FromBody] ExamDTO examDTO)
        {
            try
            {
                var exam = _mapper.ExamDTOtoExam(examDTO);
                if(_ctx.Exams.Find(examDTO.CourseTipeId, examDTO.Date) != null)
                {
                    _logger.LogError($"Post exam {exam.Id} already exists");
                    return BadRequest($"Post exam {exam.Id} already exists");
                }
                exam.Id = 0;
                _ctx.Exams.Add(exam);
                _ctx.SaveChanges();
                _logger.LogInformation($"Post exam {exam.Id}");
                return Ok(exam);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Post Exam error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
