using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPuzzle.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("course/mcq-categories")]
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

            return Ok(list);
        }
        [HttpGet("course/interview-categories")]
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

            return Ok(list);
        }

        [HttpGet("tag/coding-categories")]
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

            return Ok(list);
        }

        [HttpGet("coding-languages")]
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
            return Ok(list);
        }
    }
}

