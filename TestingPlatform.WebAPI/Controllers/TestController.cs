using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.DTO;
using TestingPlatform.Application.Interfaces;

namespace TestingPlatform.WebAPI.Controllers
{
    [ApiController]
    [EnableCors("fromUI")]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ITokenService _tokenService;

        public TestController(
            ITestService testService, 
            ITokenService tokenService)
        {
            _testService = testService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string attribute = "name", string order = "asc", int page = 1, int pageSize = 10)
        {
            var tests = await _testService.GetAllAsync(page, pageSize, attribute, order);

            if(!tests.Any())
            {
                return NotFound(page);
            }

            return new JsonResult(tests);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var test = await _testService.GetAsync(id);
            
            if (test == null)
            {
                return NotFound(id);
            }
            
            return new JsonResult(test);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]TestDTO test)
        {
            var isSuccessful = await _testService.AddAsync(test);

            if(!isSuccessful)
            {
                return BadRequest(test);
            }

            return Ok();
        }
        
        [HttpPatch("{id}")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TestDTO patchTest)
        {
            if (patchTest == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dog = await _testService.GetAsync(id);

            if (dog == null)
            {
                return NotFound();
            }

            await _testService.UpdateAsync(id, patchTest);

            return HttpContext.Request.Method switch
            {
                "PATCH" => NoContent(),
                "PUT" => Ok(patchTest),
                _ => BadRequest("Unsupported HTTP method"),
            };
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool isDeleted = await _testService.DeleteAsync(id);

            int status = isDeleted ? 1 : 0;

            switch (status)
            {
                case 1:
                    return Ok();
                case 0:
                    return BadRequest(id);
                default:
                    return StatusCode(500, "Unexpected error");
            }
        }
        
        [HttpGet("{testId}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionReturnDTO>>> GetQuestions(int testId)
        {
            var questions = await _testService.GetQuestionsForTest(testId);

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }
        
        [Authorize]
        [HttpPost("{testId}/submit")]
        public async Task<IActionResult> SubmitAnswers(int testId, [FromBody]List<AnswerSubmitDTO> answers)
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            bool result = await _testService.SubmitAnswers(testId, userEmail, answers);

            if (result)
            {
                return Ok();
            }

            return BadRequest(answers);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDTO>>> GetTestsByUser()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            
            

            return Ok();
        }
    }
}
