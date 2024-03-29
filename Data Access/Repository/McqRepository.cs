using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace InterviewPuzzle.Data_Access.Repository
{
    public class McqRepository
    {
        private readonly InterviewPuzzleDbContext _context;

        public McqRepository(InterviewPuzzleDbContext context)
        {
            _context = context;
        }

        public void AddMCQ(MCQ mcq)
        {
            _context.mcqs.Add(mcq);
        }

        public async Task<List<MCQ>> GetAllMcqAsync()
        {
            return await _context.mcqs.Include(m => m.Options).ToListAsync();
        }
        public async Task<MCQ> GetMcqAsync(int id)
        {
            return await _context.mcqs.Include(m => m.Options).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<List<MCQ>> GetMcqByCourseAsync(string course)
        {
            return await _context.mcqs.Include(q => q.Options).Where(q => q.CourseName.ToLower() ==  course.ToLower()).ToListAsync();
        }
        public async Task DeleteMcq(int id)
        {
            var mcq = await _context.mcqs.FindAsync(id);
            if (mcq == null)
                throw new NotFoundException("Mcq not found");

            _context.Remove(mcq);
        }

        public async Task DeleteMcqOption(int mcqId, int optionId)
        {
            var mcq = await _context.mcqs.FindAsync(mcqId);
            if (mcq == null)
                throw new NotFoundException("Mcq not found");

            var optionToRemove = mcq.Options.FirstOrDefault(o => o.Id == optionId);

            if (optionToRemove != null)
            {
                _context.options.Remove(optionToRemove);
            }
        }
        
        public async Task<MCQ> FindMcqAsync(int id)
        {
            return await _context.mcqs.FindAsync(id);
        }

        public void UpdateMcq(MCQ mcq)
        {
            _context.mcqs.Update(mcq);
        }
        
        public async Task<bool> IsMcqExist(string course, string question)
        {
            var result = await _context.mcqs.FirstOrDefaultAsync(m => m.CourseName.ToLower() ==  course.ToLower() && m.Question.ToLower() == question.ToLower());
            if(result == null)
                return false;
            return true;
        }
    }
}
