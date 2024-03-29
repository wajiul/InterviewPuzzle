using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access.Context;
using Microsoft.EntityFrameworkCore;

namespace InterviewPuzzle.Data_Access.Repository
{
    public class CategoryGroup
    {
        public string Source { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }

    public class InterviewRepository
    {
        private readonly InterviewPuzzleDbContext _context;

        public InterviewRepository(InterviewPuzzleDbContext context)
        {
            _context = context;
        }

        
        public async Task<List<CategoryDto>> GetMcqCategory()
        {
            var categories = await _context.mcqs
                .GroupBy(mcq => mcq.CourseName)
                .Select(g => new CategoryDto
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return categories;
        }


        public async Task<List<CategoryDto>> GetVivaQuestionCategory()
        {
            var categories = await _context.vivaQuestions
                .GroupBy(mcq => mcq.CourseName)
                .Select(g => new CategoryDto
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return categories;
        }


        public async Task<List<CategoryDto>> GetCodingQuestionCategory()
        {
            var categories = await _context.codingQuestions
                .GroupBy(mcq => mcq.Tag)
                .Select(g => new CategoryDto
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return categories;
        }

        public async Task<Dictionary<string, List<CategoryDto>>> GetCategories()
        {
            var mcqCategories = await GetMcqCategory();
            var vivaCategories = await GetVivaQuestionCategory();
            var codingCategories = await GetCodingQuestionCategory();
            var categories = new Dictionary<string, List<CategoryDto>>();

            categories.Add("MCQ", mcqCategories);
            categories.Add("Viva", vivaCategories);
            categories.Add("Coding", codingCategories);
            return categories;
        }

    }
}
