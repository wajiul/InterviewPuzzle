namespace InterviewPuzzle.Data_Access
{
    public interface IUnitOfWork
    {
        Task Complete();
    }
}