using System.Reflection;
using SEProject2.Domain;
using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Step 1: Load a program");

        Console.WriteLine($"  1) Example 1");
        Console.WriteLine($"  2) Example 2");
        Console.WriteLine($"  3) Example 3");
        Console.WriteLine($"  4) Import from another text file");

        Console.Write($"Choose [1-4]: ");

        string? choice = Console.ReadLine();
        UserProgram? prog = null;

        try
        {
            if (!int.TryParse(choice, out int choiceNumber))
            {
                Console.WriteLine("Invalid choice. Exiting.");
                return;
            }

            if (choiceNumber == 1)
            {
                prog = UserProgram.Example1();
            } else if (choiceNumber == 2)
            { 
                prog = UserProgram.Example2();
            }
            else if (choiceNumber == 3)
            {
                prog = UserProgram.Example3();
            }
            else if (choiceNumber == 4)
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
