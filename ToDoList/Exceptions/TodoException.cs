namespace ToDoList.Exceptions
{
    public class TodoException : Exception
    {
        public TodoException(string message) : base(message) { }
        public TodoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
