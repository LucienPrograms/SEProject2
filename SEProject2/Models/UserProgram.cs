using SEProject2.Domain;
using SEProject2.Service;

namespace SEProject2.Models;

public class UserProgram(string name, List<ICommand> commands)
{
    public string Name { get; set; } = name;
    public List<ICommand> Commands { get; set; } = commands;
    public Character Character { get; set; } = new();

    public void Execute()
    {
        Character.ResetToStart();  // Always reset to start before executing
        foreach (ICommand command in Commands)
        {
            command.Execute(Character);
        }

        string commandTrace = string.Join(", ", Character.GetTraceEntries()) + ".";
        Console.WriteLine(commandTrace);
        Console.WriteLine($"End state {Character.GetState()}.");
    }

    public ProgramMetrics CalculateMetrics()
    {
        var metrics = new ProgramMetrics();
        metrics.Analyze(this);
        return metrics;
    }
    
    private static string example1 = "Move 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right";
    private static string example2 = "Repeat 4 times\n    Move 10\n    Turn right";
    private static string example3 = "Move 5\nTurn left\nTurn left\nMove 3\nTurn right\nRepeat 3 times\n    Move 1\n    Turn right\n    Repeat 5 times\n        Move 2\nTurn left";

    public static UserProgram Example1() =>
        new UserProgram("Example 1", CommandFactory.FromLines(LineParser.ParseBlock(example1.Split('\n'))));
    public static UserProgram Example2() =>
        new UserProgram("Example 2", CommandFactory.FromLines(LineParser.ParseBlock(example2.Split('\n'))));
    public static UserProgram Example3() =>
        new UserProgram("Example 3", CommandFactory.FromLines(LineParser.ParseBlock(example3.Split('\n'))));
}