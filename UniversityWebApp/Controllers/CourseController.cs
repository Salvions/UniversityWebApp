using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityWebApp.DATA;
using UniversityWebApp.DTO;

namespace UniversityWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        public readonly ILogger<CourseController> _logger;
        public readonly UniversityDbContext _ctx;
        public readonly Mapper _mapper;
        public CourseController(ILogger<CourseController> logger, UniversityDbContext ctx, Mapper mapper)
        {
            _logger = logger;
            _ctx = ctx;
            _mapper = mapper;
        }

        #region GET REQUESTS
        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _ctx.Courses.ToList();
            _logger.LogInformation("GetAll courses");
            return Ok(courses.ConvertAll(_mapper.CourseToCourseDTO));
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            try
            {
                var course = _ctx.Courses.SingleOrDefault(x => x.Id == id);
                if (course == null)
                {
                    _logger.LogInformation($"Courses {id} not exists");
                    return BadRequest($"Courses {id} not exists");
                }
                CourseDTO courseDTO = _mapper.CourseToCourseDTO(course);
                _logger.LogInformation($"Courses {id}");
                return Ok(courseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Course error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Subject")]
        public IActionResult GetSubject(int id)
        {
            try
            {
                var course = _ctx.Courses.Include(x => x.CourseTipes)
                    .SingleOrDefault(x => x.Id == id);
                if (course == null)
                {
                    _logger.LogInformation($"Courses {id} not exists");
                    return BadRequest($"Courses {id} not exists");
                }
                CourseDTO courseDTO = _mapper.CourseToCourseDTO(course);
                courseDTO.CourseTipeDTOs = course.CourseTipes.ConvertAll(_mapper.CourseTipeToCourseTipeDTO);
                _logger.LogInformation($"Courses {id}");
                return Ok(courseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Course error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region POST REQUESTS
        [HttpPost]
        public IActionResult Post([FromBody] CourseDTO courseDTO)
        {
            try
            {
                Course course = _mapper.CourseDTOtoCourse(courseDTO);
                if (_ctx.Courses.Find(course.Id) != null)
                {
                    _logger.LogInformation($"Course {course.Id} already exists");
                    return BadRequest($"Course {course.Id} already exists");
                }
                _ctx.Courses.Add(course);
                _ctx.SaveChanges();
                _logger.LogInformation($"Course {course.Id} created");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create Course error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("AddSubject")]
        public IActionResult Post([FromBody] CourseTipeDTO courseTipeDTO)
        {
            try
            {
                var ct = _mapper.CourseTipeDTOtoCourseTipe(courseTipeDTO);
                if(_ctx.CourseTipes.Find(ct.CourseId, ct.SubjectId) != null)
                {
                    _logger.LogInformation("Conflict");
                    return BadRequest("Conflict");
                }
                var course= _ctx.Courses.Find(ct.CourseId);
                if (course == null)
                {
                    _logger.LogInformation($"Course {ct.CourseId} not exists");
                    return BadRequest($"Course {ct.CourseId} not exists");
                }
                if(course.CourseTipes.Sum(x => x.Credits) + ct.Credits > 130)
                {
                    _logger.LogInformation("Course credits limit reached");
                    return BadRequest("Course credits limit reached");
                }
                _ctx.CourseTipes.Add(ct);
                _ctx.SaveChanges();
                _logger.LogInformation("CourseTipe added");
                return Ok();
            }
            catch
            (Exception ex)
            {
                _logger.LogError($"Create CourseTipe error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("CloneCourse")]
        public IActionResult CloneCourse([FromBody] CourseDTO courseDTO)
        {
            var courseTipes = _ctx.CourseTipes.Where(x=>x.CourseId==courseDTO.Id);
            var course = _mapper.CourseDTOtoCourse(courseDTO);
            if(_ctx.Courses.Find(course.Id) == null)
            {
                _logger.LogInformation($"Course {course.Id} already exists");
                return BadRequest($"Course {course.Id} already exists");
            }
            Course cloneCourse = new Course()
            {
                Id = 0,
                Title = course.Title,
                Tipology = course.Tipology,
                StartDate= course.StartDate,
            };
            _ctx.Courses.Add(cloneCourse);
            _ctx.SaveChanges();
            course= _ctx.Courses.Find(cloneCourse);
            foreach (var ct in courseTipes)
            {
                var courseTipe = new CourseTipe()
                {
                    CourseId = course.Id,
                    SubjectId = ct.SubjectId,
                    Title = ct.Title,
                    Credits = ct.Credits,
                };
                _ctx.CourseTipes.Add(courseTipe);
            }
            _ctx.SaveChanges();
            return Created($"api/Course/{course.Id}", course);
        }
        #endregion

        #region DELETE REQUESTS
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var course = _ctx.Courses.Find(id);
                if (course == null)
                {
                    _logger.LogInformation($"Course {id} not exists");
                    return BadRequest($"Course {id} not exists");
                }
                _ctx.Courses.Remove(course);
                _ctx.SaveChanges();
                _logger.LogInformation($"Course {id} deleted");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete Course error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region PUT REQUESTS

        [HttpPut("AddSubject")]
        public IActionResult PutCourseTipe([FromBody] CourseTipeDTO courseTipeDTO)
        {
            try
            {
                var support = _mapper.CourseTipeDTOtoCourseTipe(courseTipeDTO);
                var courseTipe = _ctx.CourseTipes.Find(support.CourseId, support.SubjectId);
                if (courseTipe != null)
                {
                    _logger.LogError("Conflict");
                }
                _ctx.CourseTipes.Add(support);
                _ctx.SaveChanges();
                _logger.LogInformation("CourseTipe added");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Put CourseTipe error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
