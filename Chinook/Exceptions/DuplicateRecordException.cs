namespace Chinook.Exceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException() : base() { }
        public DuplicateRecordException(string message) : base(message) { }
    }
}
