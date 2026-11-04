using SEProject2.Domain;

namespace SEProject2.Models;

public class ProgramMetrics
{
    public int CommandCount { get; private set; }
    public int MaxNesting { get; private set; }
    public int RepeatCount { get; private set; }

    /// <summary>
    /// Analyses a LunarProgram with regards to specific predetermined metrics.
    /// </summary>
    /// <param name="p">The LunarProgram to analyse.</param>
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

    /// <summary>
    /// How many commands there are in total in the program, disregarding repeated commands due to repeats.
    /// </summary>
    /// <param name="commands">The ICommand commands stack.</param>
    /// <returns>The total amount of commands in the program.</returns>
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

    /// <summary>
    /// Recursively checks the depth at each part of the program for how many nested repeats there are.
    /// </summary>
    /// <param name="commands">The ICommand commands stack.</param>
    /// <param name="currentDepth">The current depth of the recursion.</param>
    /// <returns>The maximum depth of the program. I.e. the maximum amount of repeat commands inside of other repeat commands.</returns>
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

    /// <summary>
    /// Recursively counts the amount of repeat commands present in the progam.
    /// </summary>
    /// <param name="commands">The ICommand commands stack.</param>
    /// <returns>Returns the number of repeat commands.</returns>
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