using InterviewPuzzle.Data_Access.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace InterviewPuzzle.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// Gets a list of courses for MCQ.
        /// </summary>
        /// <response code="200">Returns the list of courses for MCQ.</response>

        [HttpGet("mcq-courses")]
        [ProducesResponseType(typeof(APIResponse<List<KeyValuePair<string,string>>>),200)]
        public IActionResult GetMCQCourses()
        {
            var list = new List<string>()
            {
                "All",
                "Algorithm",
                "Data Structure",
                "OOP",
                "Operating System",
                "Database",
                "Networking"
            };

            var response = new APIResponse<List<KeyValuePair<string, string>>>(true, HttpStatusCode.OK, ConvertToPair(list));

            return Ok(response);
        }

        /// <summary>
        /// Gets a List of courses for interview
        /// </summary>
        /// <response code="200">Returns the list of courses in key-value pair format for MCQ.</response>
        [HttpGet("interview-courses")]
        [ProducesResponseType(typeof(APIResponse<List<KeyValuePair<string, string>>>), 200)]
        public IActionResult GetVivaCourses()
        {
            var list = new List<string>()
            {
                "All",
                "Algorithm",
                "Data Structure",
                "OOP",
                "Operating System",
                "Database",
                "Networking"
            };

            var response = new APIResponse<List<KeyValuePair<string, string>>>(true, HttpStatusCode.OK, ConvertToPair(list));

            return Ok(response);
        }

        /// <summary>
        /// Gets a list of tags for coding problems
        /// </summary>
        /// <response code="200">Returns the list of tags in key-value pair format for coding problems.</response>

        [HttpGet("coding-tags")]
        [ProducesResponseType(typeof(APIResponse<List<KeyValuePair<string, string>>>), 200)]
        public IActionResult GetCodingTags()
        {
            var list = new List<string>()
            {
                "All",
                "Array",
                "String",
                "Linked List",
                "Stack",
                "Queue",
                "Tree",
                "Graph",
                "Dynamic Programming",
                "Recursion",
                "Sorting",
                "Searching",
                "Binary Search",
                "Hash Table",
                "Greedy",
                "Bit Manipulation",
                "Divide and Conquer",
                "Backtracking",
                "Two Pointers",
                "Math"
            };

            var response = new APIResponse<List<KeyValuePair<string, string>>>(true, HttpStatusCode.OK, ConvertToPair(list));

            return Ok(response);
        }

        /// <summary>
        /// Returns a list of coding languages
        /// </summary>
        /// <response code="200">Returns the list of programming langulages in key-value pair format.</response>
        [HttpGet("coding-languages")]
        [ProducesResponseType(typeof(APIResponse<List<KeyValuePair<string, string>>>), 200)]
        public IActionResult GetCodingLanguages()
        {
            var list = new List<string>()
            {
                "C",
                "C++",
                "C#",
                "Java",
                "JavaScript",
                "Python",
                "PHP",
                "TypeScript",
                "Ruby",
                "Swift"
            };
            var response = new APIResponse<List<KeyValuePair<string, string>>>(true, HttpStatusCode.OK, ConvertToPair(list));

            return Ok(response);
        }
        private List<KeyValuePair<string,string>> ConvertToPair(List<string> list)
        {
            var result = new List<KeyValuePair<string, string>>();

            foreach (var course in list)
            {
                var key = course.ToLower().Replace(" ", "-");
                result.Add(new KeyValuePair<string, string>(course, key));
            }
            return result;
        }
    }

}

