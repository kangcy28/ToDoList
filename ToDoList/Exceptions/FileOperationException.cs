namespace ToDoList.Exceptions
{
    public class FileOperationException : Exception
    {
        public FileOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
