using SEProject2.Domain;
using SEProject2.Service;

namespace SEProject2.Models;

public class LunarProgram(string name, List<ICommand> commands, Space? asteroidField = null)
{
    public string Name { get; set; } = name;
    public List<ICommand> Commands { get; set; } = commands;
    public Luna Character { get; private set; } = 
        asteroidField == null? new Luna() : new Luna(asteroidField);

    /// <summary>
    /// Runs the LunarProgram by executing the stored list of commands in order.
    /// </summary>
    public void Execute()
    {
        Character.ResetToStart();  // Always reset to start before executing

        foreach (ICommand command in Commands)
        {
            command.Execute(Character);
        }
    }

    /// <summary>
    /// How many commands there are in total in the program, disregarding repeated commands due to repeats.
    /// </summary>
    /// <returns>The total amount of commands in the program.</returns>
    public int CountCommands() => CountCommandsRecursive(Commands);
    /// <summary>
    /// Recursively checks the depth at each part of the program for how many nested repeats there are.
    /// </summary>
    /// <returns>The maximum depth of the program. I.e. the maximum amount of repeat commands inside of other repeat commands.</returns>
    public int MaxNesting() => ComputeMaxNesting(Commands, 0);
    /// <summary>
    /// Recursively counts the amount of repeat commands present in the progam.
    /// </summary>
    /// <returns>Returns the number of repeat commands.</returns>
    public int CountRepeats() => CountRepeatsRecursive(Commands);
    
    private static int CountCommandsRecursive(IEnumerable<ICommand> commands)
    {
        int total = 0;
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
    
    private static int ComputeMaxNesting(IEnumerable<ICommand> commands, int currentDepth)
    {
        int maxDepth = currentDepth;
        foreach (var command in commands)
        {
            var subCommands = command switch
            {
                RepeatUntilCommand untilCommand => untilCommand.SubCommands,
                RepeatCommand repeatCommand => repeatCommand.SubCommands,
                _ => null
            };
            if (subCommands == null) continue;
            int childDepth = ComputeMaxNesting(subCommands, currentDepth + 1);
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

    public const string _example1 = "Move 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right";
    public const string _example2 = "Repeat 4 times\n    Move 10\n    Turn right";
    public const string _example3 = "Move 5\nTurn left\nTurn left\nMove 3\nTurn right\nRepeat 3 times\n    Move 1\n    Turn right\n    Repeat 5 times\n        Move 2\nTurn left";

    public static LunarProgram Example1() =>
        new("Example 1", CommandFactory.FromLines(LineParser.ParseBlock(_example1.Split('\n'))));
    public static LunarProgram Example2() =>
        new("Example 2", CommandFactory.FromLines(LineParser.ParseBlock(_example2.Split('\n'))));
    public static LunarProgram Example3() =>
        new("Example 3", CommandFactory.FromLines(LineParser.ParseBlock(_example3.Split('\n'))));
}