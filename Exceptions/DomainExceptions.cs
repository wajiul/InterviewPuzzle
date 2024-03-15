namespace InterviewPuzzle.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }

    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string message) : base(message)
        {

        }
    }

    public class NotExistException : Exception
    {
        public NotExistException(string message) : base(message)
        {

        }
    }

}
