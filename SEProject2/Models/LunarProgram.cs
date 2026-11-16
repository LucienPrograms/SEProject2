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

    private const string example1 = "Move 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right";
    private const string example2 = "Repeat 4 times\n    Move 10\n    Turn right";
    private const string example3 = "Move 5\nTurn left\nTurn left\nMove 3\nTurn right\nRepeat 3 times\n    Move 1\n    Turn right\n    Repeat 5 times\n        Move 2\nTurn left";

    public static LunarProgram Example1() =>
        new("Example 1", CommandFactory.FromLines(LineParser.ParseBlock(example1.Split('\n'))));
    public static LunarProgram Example2() =>
        new("Example 2", CommandFactory.FromLines(LineParser.ParseBlock(example2.Split('\n'))));
    public static LunarProgram Example3() =>
        new("Example 3", CommandFactory.FromLines(LineParser.ParseBlock(example3.Split('\n'))));
}