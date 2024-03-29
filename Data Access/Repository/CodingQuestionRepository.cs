using InterviewPuzzle.Data_Access.Context;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace InterviewPuzzle.Data_Access.Repository
{
    public class CodingQuestionRepository
    {
        private readonly InterviewPuzzleDbContext _context;

        public CodingQuestionRepository(InterviewPuzzleDbContext context)
        {
            _context = context;
        }

        public async Task<List<CodingQuestion>> GetCodingQuestionsAsync()
        {
            return await _context.codingQuestions.Include(c => c.Solutions).ToListAsync();
        }
        public async Task<CodingQuestion> GetCodingQuestionAsync(int id)
        {
            return await _context.codingQuestions.Include(q => q.Solutions).FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<List<CodingQuestion>> GetCodingQuestionsByTagAsync(string tag)
        {
            return await _context.codingQuestions.Include(q => q.Solutions).Where(q => q.Tag.ToLower() == tag.ToLower()).ToListAsync();
        }

        public void Add(CodingQuestion codingQuestion)
        {
            _context.codingQuestions.Add(codingQuestion);   
        }

        public async Task<bool> IsQuestionExist(string question)
        {
            var result = await _context.codingQuestions.FirstOrDefaultAsync(q => q.Text == question);
            return result != null;
        }
        public void UpdateCodingQuestion(CodingQuestion codingQuestion)
        {
            _context.codingQuestions.Update(codingQuestion);
        }
        public void DeleteCodingQuestion(CodingQuestion codingQuestion)
        {
            _context.codingQuestions.Remove(codingQuestion);
        }

        public async Task DeleteSolution(int questionId, int solutionId)
        {
            var question = await _context.codingQuestions.Include(q => q.Solutions).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
                throw new NotFoundException($"Coding Question with Id {questionId} not found.");

            var solutionToRemove = question.Solutions.FirstOrDefault(s => s.Id == solutionId);

            if (solutionToRemove == null)
            {
                throw new NotFoundException($"Coding Question with Id {questionId} not found.");
            }

             _context.solutions.Remove(solutionToRemove);
        }

    }
}
