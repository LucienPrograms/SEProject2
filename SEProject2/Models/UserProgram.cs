using SEProject2.Domain;

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
    
}