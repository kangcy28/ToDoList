namespace ToDoList.Exceptions
{
    public class ValidationException : TodoException
    {
        public ValidationException(string message) : base(message) { }
    }
}
