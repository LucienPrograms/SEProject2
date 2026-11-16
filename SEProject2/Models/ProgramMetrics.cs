using SEProject2.Domain;

namespace SEProject2.Models;

public record ProgramMetrics
{
    private ProgramMetrics(int maxNesting, int commandCount, int repeatCommandCount)
    {
        MaxNesting = maxNesting;
        CommandCount = commandCount;
        RepeatCommandCount = repeatCommandCount;
    }

    public int MaxNesting { get; }
    public int CommandCount { get; }
    public int RepeatCommandCount { get; }

    public static ProgramMetrics FromProgram(LunarProgram p) => new(ComputeMaxNesting(p.Commands, 0), CountCommandsRecursive(p.Commands), CountRepeatsRecursive(p.Commands));

    /// <summary>
    /// How many commands there are in total in the program, disregarding repeated commands due to repeats.
    /// </summary>
    /// <returns>The total amount of commands in the program.</returns>
    private static int CountCommandsRecursive(IEnumerable<ICommand> commands)
    {
        var total = 0;
        foreach (var command in commands)
        {
            total += 1;
            var subCommands = command switch
            {
                RepeatUntilCommand untilCommand => untilCommand.SubCommands,
                RepeatCommand repeatCommand => repeatCommand.SubCommands,
                _ => null
            };
            if (subCommands == null) continue;
            total += CountCommandsRecursive(subCommands);
        }
        return total;
    }
    
    /// <summary>
    /// Recursively checks the depth at each part of the program for how many nested repeats there are.
    /// </summary>
    /// <returns>The maximum depth of the program. I.e. the maximum amount of repeat commands inside of other repeat commands.</returns>
    private static int ComputeMaxNesting(IEnumerable<ICommand> commands, int currentDepth)
    {
        var maxDepth = currentDepth;
        foreach (var command in commands)
        {
            var subCommands = command switch
            {
                RepeatUntilCommand untilCommand => untilCommand.SubCommands,
                RepeatCommand repeatCommand => repeatCommand.SubCommands,
                _ => null
            };
            if (subCommands == null) continue;
            var childDepth = ComputeMaxNesting(subCommands, currentDepth + 1);
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
    /// <returns>Returns the number of repeat commands.</returns>
    private static int CountRepeatsRecursive(IEnumerable<ICommand> commands)
    {
        var repeats = 0;
        foreach (var command in commands)
        {
            var subCommands = command switch
            {
                RepeatUntilCommand untilCommand => untilCommand.SubCommands,
                RepeatCommand repeatCommand => repeatCommand.SubCommands,
                _ => null
            };
            if (subCommands == null) continue;
            repeats += 1;
            repeats += CountRepeatsRecursive(subCommands);
        }
        return repeats;
    }
};