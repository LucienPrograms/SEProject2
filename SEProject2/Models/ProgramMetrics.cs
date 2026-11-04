using SEProject2.Domain;

namespace SEProject2.Models;

public class ProgramMetrics
{
    public int CommandCount { get; private set; }
    public int MaxNesting { get; private set; }
    public int RepeatCount { get; private set; }

    public void Analyze(LunarProgram p)
    {
        if (p == null)
        {
            CommandCount = 0;
            MaxNesting = 0;
            RepeatCount = 0;
            return;
        }

        CommandCount = CalculateCommandCount(p);
        MaxNesting = CalculateMaxNesting(p);
        RepeatCount = CalculateRepeatCount(p);

        Console.WriteLine($"ICommand count: {CommandCount}");
        Console.WriteLine($"Max nesting: {MaxNesting}");
        Console.WriteLine($"Repeat count: {RepeatCount}");
    }

    private static int CalculateCommandCount(LunarProgram p) => CountCommandsRecursive(p.Commands);

    private static int CalculateMaxNesting(LunarProgram p) => ComputeMaxNesting(p.Commands, 0);

    private static int CalculateRepeatCount(LunarProgram p) => CountRepeatsRecursive(p.Commands);

    private static int CountCommandsRecursive(IEnumerable<ICommand> commands)
    {
        int total = 0;
        foreach (var command in commands)
        {
            total += 1;
            if (command is RepeatCommand repeat)
            {
                total += CountCommandsRecursive(repeat.SubCommands);
            }
        }
        return total;
    }

    private static int ComputeMaxNesting(IEnumerable<ICommand> commands, int currentDepth)
    {
        int maxDepth = currentDepth;
        foreach (var command in commands)
        {
            if (command is not RepeatCommand repeat) continue;
            int childDepth = ComputeMaxNesting(repeat.SubCommands, currentDepth + 1);
            if (childDepth > maxDepth)
            {
                maxDepth = childDepth;
            }
        }
        return maxDepth;
    }

    private static int CountRepeatsRecursive(IEnumerable<ICommand> commands)
    {
        int repeats = 0;
        foreach (var command in commands)
        {
            if (command is not RepeatCommand repeat) continue;
            repeats += 1;
            repeats += CountRepeatsRecursive(repeat.SubCommands);
        }
        return repeats;
    }
}