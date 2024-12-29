// Services/FileService.cs
using System.Text.Json;
using ToDoList.Exceptions;
using ToDoList.Models;

public class FileService
{
    private readonly string _defaultFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public FileService()
    {
        // Create a 'Data' directory in the application's root
        string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        Directory.CreateDirectory(dataDirectory); // Creates if doesn't exist
        _defaultFilePath = Path.Combine(dataDirectory, "todos.json");

        // Initialize JSON options once in constructor
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true  // More flexible JSON parsing
        };
    }

    public async Task SaveTodosAsync(IEnumerable<Todo> todos, string filePath = null)
    {
        filePath ??= _defaultFilePath;

        // Ensure directory exists
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            Console.WriteLine("\nhere is the todos", todos);
            Console.WriteLine("");
            string jsonData = JsonSerializer.Serialize(todos, _jsonOptions);
            await File.WriteAllTextAsync(filePath, jsonData);
        }
        catch (Exception ex)
        {
            throw new FileOperationException($"Failed to save todos to {filePath}", ex);
        }
    }

    public async Task<List<Todo>> LoadTodosAsync(string filePath)
    {
        filePath ??= _defaultFilePath;

        try
        { 
            if (!File.Exists(filePath))
            { 
                Console.WriteLine($"File not found at path: {filePath}");
                return new List<Todo>();
            }
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Debug output
            Console.WriteLine("JSON Content:");
            Console.WriteLine(jsonData);
            Console.WriteLine($"JSON Length: {jsonData.Length}");

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                Console.WriteLine("JSON data is empty");
                return new List<Todo>();
            }

            var todos = JsonSerializer.Deserialize<List<Todo>>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var todo in todos ?? new List<Todo>())
            {
                todo.Validate();
            }

            return todos ?? new List<Todo>();
        }
        catch (JsonException ex)
        {
            throw new FileOperationException("Invalid JSON format in file", ex);
        }
        catch (Exception ex)
        {
            throw new FileOperationException($"Failed to load todos from {filePath}", ex);
        }
    }
}