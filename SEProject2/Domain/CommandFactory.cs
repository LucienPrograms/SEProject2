using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2.Domain;

public static class CommandFactory
{
    // Keywords
    private const string Move = "Move";
    private const string RepeatCond = "RepeatUntil";
    private const string Turn = "Turn";
    private const string Repeat = "Repeat";
    private const string Right = "right";
    private const string Left = "left";
    private const string Times = "times";
    
    private const string WallAhead = "GridAhead";
    private const string GridEdge = "GridEdge";

    private static RepeatCondition? StringToCondition(string s) => 
        s switch 
        {
            WallAhead => RepeatCondition.WallAhead,
            GridEdge => RepeatCondition.GridEdge,
            _ => null
        };

    /// <summary>
    /// Creates move command.
    /// </summary>
    /// <param name="line">Command line.</param>
    /// <returns>New move command.</returns>
    private static MoveCommand ParseMoveCommand(ParsedLine line)
    {
        var steps = uint.Parse(line.Parts[1]);
        return CreateMove(steps);
    }

    /// <summary>
    /// Creates turn command.
    /// </summary>
    /// <param name="line">Command line.</param>
    /// <returns>New turn command.</returns>
    /// <exception cref="Exception">Direction to turn is invalid. Must be 'Right' or 'Left'.</exception>
    private static TurnCommand ParseTurnCommand(ParsedLine line)
    {
        Direction.LeftRight direction = line.Parts[1] switch
        {
            Right => Direction.LeftRight.Right,
            Left => Direction.LeftRight.Left,
            _ => throw new Exception("Invalid turn")
        };

        return CreateTurn(direction);
    }

    private static List<ICommand> ParseSubCommands(Queue<ParsedLine> lines, ParsedLine line)
    {
        var subCommands = new List<ICommand>();
        
        while (lines.Count > 0 && lines.Peek().Indent > line.Indent)
        {
            subCommands.Add(ParseCommand(lines));
        }
        
        return subCommands;
    }

    private static RepeatCommand ParseRepeatCommand(Queue<ParsedLine> lines, ParsedLine line)
    {
        var times = uint.Parse(line.Parts[1]);
        if (line.Parts[2] != Times) throw new Exception("Malformed syntax");
        var subCommands = ParseSubCommands(lines, line);
        return CreateRepeat(times, subCommands);
    }
    
    private static RepeatUntilCommand ParseRepeatUntilCommand(Queue<ParsedLine> lines, ParsedLine line)
    {
        var condition = StringToCondition(line.Parts[1]) ?? throw new Exception("Malformed syntax: invalid condition");
        var subCommands = ParseSubCommands(lines, line);
        return CreateRepeatUntil(condition, subCommands);
    }
    
    /// <summary>
    /// Parses a command line and creates the respective command.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns>New ICommand command.</returns>
    /// <exception cref="Exception">Command syntax is invalid or could not be parsed.</exception>
    private static ICommand ParseCommand(Queue<ParsedLine> lines)
    {
        var line = lines.Dequeue();
        return line.Parts[0] switch
        {
            Move => ParseMoveCommand(line),
            Turn => ParseTurnCommand(line),
            RepeatCond => ParseRepeatUntilCommand(lines, line),
            Repeat => ParseRepeatCommand(lines, line),
            _ => throw new Exception("Malformed syntax")
        };
    }

    /// <summary>
    /// Parses through all command lines to create an ordered list of ICommands.
    /// </summary>
    /// <param name="lines">Command lines to parse.</param>
    /// <returns>List of ICommand commands.</returns>
    public static List<ICommand> FromLines(IReadOnlyList<ParsedLine> lines)
    {
        var commands = new List<ICommand>();
        var queue = new Queue<ParsedLine>(lines);
        
        while (queue.Count > 0)
        {
            commands.Add(ParseCommand(queue));
        }
        
        return commands;
    }
    
    public static RepeatUntilCommand CreateRepeatUntil(RepeatCondition condition, IReadOnlyList<ICommand> subCommands) => new(condition, subCommands);
    public static MoveCommand CreateMove(uint steps) => new(steps);
    public static TurnCommand CreateTurn(Direction.LeftRight direction) => new(direction);

    public static RepeatCommand CreateRepeat(uint amount, IReadOnlyList<ICommand> subCommands) => new(amount, subCommands);
}