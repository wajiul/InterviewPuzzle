using InterviewPuzzle.Data_Access.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterviewPuzzle.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// Returns a List of courses for MCQ
        /// </summary>
        /// <returns></returns>
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
        /// Returns a List of courses for interview
        /// </summary>
        /// <returns></returns>
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
        /// Returns a list of tags for coding problems
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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

