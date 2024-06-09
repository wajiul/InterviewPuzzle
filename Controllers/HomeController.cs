using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace InterviewPuzzle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class HomeController : ControllerBase
    {
        private readonly InterviewRepository _repository;

        public HomeController(InterviewRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets topics or tags for each course.
        /// </summary>
        /// <returns>Returns a list of topics or tags for each course.</returns>
        /// <response code="200">Returns the list of topics or tags.</response>
        [HttpGet]
        [Route("course-analytics")]
        [ProducesResponseType(typeof(APIResponse<Dictionary<string, List<CategoryDto>>>), 200)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _repository.GetCategories();
            return Ok(new APIResponse<Dictionary<string, List<CategoryDto>>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = categories
            });
        }


    }
}



