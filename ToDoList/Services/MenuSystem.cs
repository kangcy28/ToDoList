using ToDoList.Exceptions;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class MenuSystem
    {
        private readonly TodoManager _todoManager;

        public MenuSystem(TodoManager todoManager)
        {
            _todoManager = todoManager;
        }

        public async Task RunAsync()
        {
            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateTodoAsync();
                        break;
                    case "2":
                        await DisplayAllTodosAsync();
                        break;
                    case "3":
                        await DisplaySingleTodoAsync();
                        break;
                    case "4":
                        await UpdateTodoAsync();
                        break;
                    case "5":
                        await DeleteTodoAsync();
                        break;
                    case "6":
                        await ToggleTodoStatusAsync();
                        break;
                    case "7":
                        await SaveToFileAsync();
                        break;
                    case "8":
                        await LoadFromFileAsync();
                        break;
                    case "9":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Todo List Manager ===");
            Console.WriteLine("1. Add New Todo");
            Console.WriteLine("2. View All Todos");
            Console.WriteLine("3. View Single Todo");
            Console.WriteLine("4. Update Todo");
            Console.WriteLine("5. Delete Todo");
            Console.WriteLine("6. Toggle Todo Status");
            Console.WriteLine("7. Save to File");
            Console.WriteLine("8. Load from File");
            Console.WriteLine("9. Exit");
            Console.Write("Enter your choice (1-9): ");
        }

        private async Task CreateTodoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Create New Todo ===");

            try
            {
                Console.Write("Enter title: ");
                string title = Console.ReadLine();

                Console.Write("Enter description: ");
                string description = Console.ReadLine();

                var todo = await _todoManager.AddTodoAsync(title, description);
                Console.WriteLine($"\nTodo created successfully! ID: {todo.Id}");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"\nValidation Error: {ex.Message}");
            }
            catch (TodoException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DisplayAllTodosAsync()
        {
            Console.Clear();
            Console.WriteLine("=== All Todos ===\n");

            try
            {
                var todos = await _todoManager.GetAllTodosAsync();
                if (!todos.Any())
                {
                    Console.WriteLine("No todos found.");
                }
                else
                {
                    foreach (var todo in todos)
                    {
                        DisplayTodo(todo);
                        Console.WriteLine("-------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving todos: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DisplaySingleTodoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== View Single Todo ===\n");

            Console.Write("Enter todo ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var todo = await _todoManager.GetTodoByIdAsync(id);
                    if (todo != null)
                    {
                        DisplayTodo(todo);
                    }
                    else
                    {
                        Console.WriteLine("Todo not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving todo: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void DisplayTodo(Todo todo)
        {
            Console.WriteLine($"ID: {todo.Id}");
            Console.WriteLine($"Title: {todo.Title}");
            Console.WriteLine($"Description: {todo.Description}");
            Console.WriteLine($"Status: {(todo.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine($"Created: {todo.CreatedDate}");
        }
        private async Task UpdateTodoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Update Todo ===\n");

            Console.Write("Enter todo ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                var todo = await _todoManager.GetTodoByIdAsync(id);
                if (todo == null)
                {
                    Console.WriteLine("Todo not found.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"Current Title: {todo.Title}");
                Console.Write("Enter new title (or press Enter to keep current): ");
                string title = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(title))
                    title = todo.Title;

                Console.WriteLine($"Current Description: {todo.Description}");
                Console.Write("Enter new description (or press Enter to keep current): ");
                string description = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(description))
                    description = todo.Description;

                await _todoManager.UpdateTodoAsync(id, title, description);
                Console.WriteLine("\nTodo updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError updating todo: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DeleteTodoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Todo ===\n");

            Console.Write("Enter todo ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                await _todoManager.DeleteTodoAsync(id);
                Console.WriteLine("\nTodo deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError deleting todo: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ToggleTodoStatusAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Toggle Todo Status ===\n");

            Console.Write("Enter todo ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                bool newStatus = await _todoManager.ToggleTodoStatusAsync(id);
                Console.WriteLine($"\nTodo status updated to: {(newStatus ? "Completed" : "Pending")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError toggling todo status: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task SaveToFileAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Save Todos to File ===\n");

            try
            {
                Console.Write("Enter file path (or press Enter for default 'Data/todos.json'): ");
                string filePath = Console.ReadLine();

                // Show where the file will be saved
                string finalPath = string.IsNullOrWhiteSpace(filePath)
                    ? Path.Combine(Directory.GetCurrentDirectory(), "Data", "todos.json")
                    : filePath;
                Console.WriteLine($"\nSaving to: {finalPath}");

                // Ask for confirmation if file exists
                if (File.Exists(finalPath))
                {
                    Console.Write("\nFile already exists. Do you want to overwrite it? (y/n): ");
                    if (Console.ReadLine()?.ToLower() != "y")
                    {
                        Console.WriteLine("\nSave operation cancelled.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        return;
                    }
                }

                await _todoManager.SaveToFileAsync(finalPath);
                Console.WriteLine("\nTodos saved successfully!");
            }
            catch (FileOperationException ex)
            {
                Console.WriteLine($"\nError saving todos: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Details: {ex.InnerException.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task LoadFromFileAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Load Todos from File ===\n");

            try
            {
                Console.Write("Enter file path (or press Enter for default 'Data/todos.json'): ");
                string filePath = Console.ReadLine();

                string finalPath = string.IsNullOrWhiteSpace(filePath)
                    ? Path.Combine(Directory.GetCurrentDirectory(), "Data", "todos.json")
                    : filePath;

                if (!File.Exists(finalPath))
                {
                    Console.WriteLine($"\nFile not found: {finalPath}");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }

                // Show confirmation before loading
                Console.WriteLine($"\nLoading from: {finalPath}");
                Console.Write("This will replace all current todos. Continue? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y")
                {
                    Console.WriteLine("\nLoad operation cancelled.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }

                await _todoManager.LoadFromFileAsync(finalPath);
                Console.WriteLine("\nTodos loaded successfully!");
            }
            catch (FileOperationException ex)
            {
                Console.WriteLine($"\nError loading todos: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Details: {ex.InnerException.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
