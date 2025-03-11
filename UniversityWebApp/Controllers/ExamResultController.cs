using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityWebApp.DATA;
using UniversityWebApp.DTO;

namespace UniversityWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : ControllerBase
    {
        public readonly Mapper _mapper;
        public readonly ILogger<ExamResultController> _logger;
        public readonly UniversityDbContext _ctx;
        public ExamResultController(Mapper mapper, ILogger<ExamResultController> logger, UniversityDbContext ctx)
        {
            _mapper = mapper;
            _logger = logger;
            _ctx = ctx;
        }

        #region GET REQUESTS
        [HttpGet]
        public IActionResult GetAll()
        {
            var examResults = _ctx.ExamResults.ToList();
            _logger.LogInformation("GetAll examResults");
            return Ok(examResults.ConvertAll(_mapper.ExamResultToExamResultDTO));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var examResult = _ctx.ExamResults.Find(id);
                if (examResult == null)
                {
                    _logger.LogError($"ExamResult {id} not found");
                    return BadRequest($"ExamResult {id} not found");
                }
                _logger.LogInformation($"Get examResult {id}");
                return Ok(_mapper.ExamResultToExamResultDTO(examResult));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get ExamResult error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region POST REQUESTS
        [HttpPost]
        public IActionResult Post([FromBody] ExamResultDTO examResultDTO)
        {
            var exresult = _ctx.ExamResults;
            try
            {
                var examResult = _mapper.ExamResultDTOtoExamResult(examResultDTO);
                if (examResult == null)
                {
                    _logger.LogError($"Post examResult error");
                    return BadRequest("Post examResult error");
                }
                if(exresult.Find(examResult)!=null)
                {
                    _logger.LogError($"Post examResult conflict");
                    return Conflict();
                }
                _ctx.ExamResults.Add(examResult);
                _ctx.SaveChanges();
                _logger.LogInformation($"Post examResult {examResult}");
                return Ok(examResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Post examResult error, {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("massive")]
        public IActionResult Post([FromBody] List<ExamResultDTO> examResultsDTO)
        {
            var exresult = _ctx.ExamResults.Where(x=>x.Grade==-1).ToList();
            try
            {
                var examResults = examResultsDTO.ConvertAll(_mapper.ExamResultDTOtoExamResult);
                foreach(var ex in examResults)
                {
                    if (ex == null)
                    {
                        _logger.LogError($"Post examResult error");
                    }
                    else if (exresult.SingleOrDefault(ex) == null)
                    {
                        _logger.LogError($"Post Student not Registred");
                    }
                    else if(exresult.Find(x=>x.StudentId == ex.StudentId 
                            && x.ExamId == ex.ExamId) ==null)
                    {
                        _logger.LogError($"Post examResult conflict");
                    }
                    else
                    {
                        _ctx.ExamResults.Add(ex);
                        _logger.LogInformation($"Post examResult {ex}");
                    }
                }
                _ctx.SaveChanges();
                return Ok(exresult);
            }
            catch
            {
                _logger.LogError($"Post examResults error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
