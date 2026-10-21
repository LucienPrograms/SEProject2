using SEProject2.Domain;
using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("MSO Lab 2 - Simple CLI\n");
        Console.WriteLine("Step 1: Load a program");
        Console.WriteLine("  1) Example: basic");
        Console.WriteLine("  2) Example: advanced");
        Console.WriteLine("  3) Example: expert");
        Console.WriteLine("  4) Import from text file");
        Console.Write("Choose [1-4]: ");
        
        string? choice = Console.ReadLine();
        Models.UserProgram? prog = null;
        
        try
        {
            switch (choice)
            {
                case "1":
                    // Basic examples
                    prog = ProgramImporter.ImportFromFile(Path.Combine("examples", "example1.txt"));
                    break;
                case "2":
                    // Advanced examples
                    prog = ProgramImporter.ImportFromFile(Path.Combine("examples", "example2.txt"));
                    break;
                case "3":
                    // Expert examples
                    prog = ProgramImporter.ImportFromFile(Path.Combine("examples", "example3.txt"));
                    break;
                case "4":
                    Console.Write("Enter path to program text file: ");
                    string? filePath = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(filePath))
                    {
                        Console.WriteLine("No file path provided. Exiting.");
                        return;
                    }
                    prog = ProgramImporter.ImportFromFile(filePath);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Exiting.");
                    return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load program: {ex.Message}");
            return;
        }
        
        if (prog == null)
        {
            Console.WriteLine("No program loaded. Exiting.");
            return;
        }
        
        Console.WriteLine($"\nLoaded program: {prog.Name}");
        Console.WriteLine("Step 2: What do you want to do?");
        Console.WriteLine("  1) Execute program");
        Console.WriteLine("  2) Calculate metrics");
        Console.Write("Choose [1-2]: ");
        
        string? action = Console.ReadLine();
        Console.WriteLine();
        switch (action)
        {
            case "1":
                prog.Execute();
                break;
            case "2":
                prog.CalculateMetrics();
                break;
            default:
                Console.WriteLine("Invalid choice. Exiting.");
                return;
        }
    }
}