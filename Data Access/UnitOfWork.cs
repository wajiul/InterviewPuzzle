using InterviewPuzzle.Data_Access.Context;

namespace InterviewPuzzle.Data_Access
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InterviewPuzzleDbContext _context;

        public UnitOfWork(InterviewPuzzleDbContext context)
        {
            _context = context;
        }
        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

    }
}
