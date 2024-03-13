using InterviewPuzzle.Data_Access.Model;
using Microsoft.EntityFrameworkCore;

namespace InterviewPuzzle.Data_Access.Context
{
    public class InterviewPuzzleDbContext: DbContext
    {
        public InterviewPuzzleDbContext(DbContextOptions<InterviewPuzzleDbContext> options): base(options)
        {
            
        }

        public DbSet<MCQ> mcqs { get; set; }
        public DbSet<Option> options { get; set; }
    }
}
