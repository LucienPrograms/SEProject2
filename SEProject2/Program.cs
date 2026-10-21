using SEProject2.Domain;
using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2;

internal static class Program
{
    private static void Main(string[] args)
    {
        string projectDirectory = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        string examplesDirectory = Path.Combine(projectDirectory, "examples");

        // Find all text files in the examples directory.
        string[] exampleFiles = Directory.Exists(examplesDirectory)
            ? Directory.GetFiles(examplesDirectory, "*.txt")
                .OrderBy(Path.GetFileName)
                .ToArray()
            : Array.Empty<string>();

        Console.WriteLine("Step 1: Load a program");

        for (int i = 0; i < exampleFiles.Length; i++)
        {
            Console.WriteLine($"  {i + 1}) {Path.GetFileName(exampleFiles[i])}");
        }

        int importOption = exampleFiles.Length + 1;
        Console.WriteLine($"  {importOption}) Import from another text file");

        Console.Write($"Choose [1-{importOption}]: ");

        string? choice = Console.ReadLine();
        UserProgram? prog = null;

        try
        {
            if (int.TryParse(choice, out int selectedOption)
                && selectedOption >= 1
                && selectedOption <= exampleFiles.Length)
            {
                prog = ProgramImporter.ImportFromFile(
                    exampleFiles[selectedOption - 1]);
            }
            else if (int.TryParse(choice, out selectedOption)
                && selectedOption == importOption)
            {
                Console.Write("Enter path to program text file: ");
                string? filePath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    Console.WriteLine("No file path provided. Exiting.");
                    return;
                }

                prog = ProgramImporter.ImportFromFile(filePath);
            }
            else
            {
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
