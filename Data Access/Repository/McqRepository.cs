using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Model;
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

        public void DeleteMcq(int id)
        {
            var mcq = _context.mcqs.Find(id);
            if (mcq == null)
                return;

            _context.Remove(mcq);
        }

        public async Task<MCQ> FindMcqAsync(int id)
        {
            return await _context.mcqs.FindAsync(id);
        }

        public void UpdateMcq(MCQ mcq)
        {
            _context.mcqs.Update(mcq);
        }
    }
}
