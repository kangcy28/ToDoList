namespace ToDoList.Exceptions
{
    public class TodoNotFoundException : TodoException
    {
        public TodoNotFoundException(int id)
            : base($"Todo with ID {id} not found.") { }
    }
}
