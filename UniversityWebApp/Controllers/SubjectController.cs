using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityWebApp.DATA;
using UniversityWebApp.DTO;

namespace UniversityWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        public readonly Mapper _mapper;
        public readonly ILogger<SubjectController> _logger;
        public readonly UniversityDbContext _ctx;

        public SubjectController(Mapper mapper, ILogger<SubjectController> logger, UniversityDbContext ctx)
        {
            _mapper = mapper;
            _logger = logger;
            _ctx = ctx;
        }

        #region GET REQUESTS

        /// <summary>
        /// ritorna tutti i subjects senza le sue relazioni
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var subjects = _ctx.Subjects.ToList();
            _logger.LogInformation("GetAll subjects");
            return Ok(subjects.ConvertAll(_mapper.SubjectToSubjectDTO));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var subject = _ctx.Subjects.Find(id);
                if (subject==null)
                {
                    _logger.LogError($"Subject {id} not found");
                    return BadRequest($"Subject {id} not found");
                }
                _logger.LogInformation($"Get subject {id}");
                return Ok(_mapper.SubjectToSubjectDTO(subject));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Subject error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Course")]
        public IActionResult GetCourse(int id)
        {
            try
            {
                var subject = _ctx.Subjects.Include(x => x.CourseTipes)
                    .SingleOrDefault(x=>x.Id == id);
                if (subject == null)
                {
                    _logger.LogError($"Subject {id} not found");
                    return BadRequest($"Subject {id} not found");
                }
                SubjectDTO subjectDTO = _mapper.SubjectToSubjectDTO(subject);
                subjectDTO.CourseTipeDTOs = subject.CourseTipes.ConvertAll(_mapper.CourseTipeToCourseTipeDTO);
                _logger.LogInformation($"Get subject {id}");
                return Ok(subjectDTO);
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
        public IActionResult Post([FromBody] SubjectDTO subjectDTO)
        {
            try
            {
                var subject = _mapper.SubjectDTOtoSubject(subjectDTO);
                _ctx.Subjects.Add(subject);
                _ctx.SaveChanges();
                _logger.LogInformation($"Post subject {subject.Id}");
                return Ok(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Post Subject error, {ex}");
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
                var subject = _ctx.Subjects.Find(id);
                if (subject == null)
                {
                    _logger.LogError($"Subject {id} not found");
                    return BadRequest($"Subject {id} not found");
                }
                _ctx.Subjects.Remove(subject);
                _ctx.SaveChanges();
                _logger.LogInformation($"Delete subject {id}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete Subject error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
