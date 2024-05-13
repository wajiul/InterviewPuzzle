using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPuzzle.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("mcq-courses")]
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

            return Ok(ConvertToPair(list));
        }

        [HttpGet("interview-courses")]
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

            return Ok(ConvertToPair(list));
        }

        [HttpGet("coding-tags")]
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

            return Ok(ConvertToPair(list));
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

