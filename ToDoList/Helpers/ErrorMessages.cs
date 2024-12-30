namespace ToDoList.Helpers
{
    public static class ErrorMessages
    {
        public static void DisplayError(string message, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {message}");

            if (ex?.InnerException != null)
            {
                Console.WriteLine($"Details: {ex.InnerException.Message}");
            }

            Console.ResetColor();
        }

        public static void DisplayValidationError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nValidation Error: {message}");
            Console.ResetColor();
        }
    }
}
