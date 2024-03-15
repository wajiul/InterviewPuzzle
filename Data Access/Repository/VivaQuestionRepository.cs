using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace InterviewPuzzle.Data_Access.Repository
{
    public class VivaQuestionRepository
    {
        private readonly InterviewPuzzleDbContext _context;

        public VivaQuestionRepository(InterviewPuzzleDbContext context)
        {
            _context = context;
        }

        public async Task<List<VivaQuestion>> GetVivaQuestionsAsync()
        {
            return await _context.vivaQuestions.ToListAsync();
        }
        public async Task<VivaQuestion> GetVivaQuestionAsync(int id)
        {
            var question = await _context.vivaQuestions.FirstOrDefaultAsync(x => x.Id == id);
            return question;
        }
        
        public void AddVavaQuestion(VivaQuestion question)
        {
            _context.vivaQuestions.Add(question);
        }
        public void UpdateVivaQuestion(VivaQuestion question)
        {
            _context.vivaQuestions.Update(question);
        }
        public void DeleteVivaQuestion(VivaQuestion question)
        {
            _context.vivaQuestions.Remove(question);
        }

        public async Task<bool> isQuestionExist(string course, string question)
        {
           var result =  await _context.vivaQuestions.FirstOrDefaultAsync(q => q.CourseName == course && q.Text == question);
            return result != null;
        }

    }
}
