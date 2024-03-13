namespace InterviewPuzzle.Exceptions
{
    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException(string message) : base(message)
        {

        }
    }

    public class DomainAlreadyExistException : Exception
    {
        public DomainAlreadyExistException(string message) : base(message)
        {

        }
    }

    public class DomainNotExistException : Exception
    {
        public DomainNotExistException(string message) : base(message)
        {

        }
    }

}
