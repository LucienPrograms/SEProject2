using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2.Domain;

public static class CommandFactory
{
    // Keywords
    private const string Move = "Move";
    private const string Turn = "Turn";
    private const string Repeat = "Repeat";
    private const string Right = "right";
    private const string Left = "left";
    private const string Times = "times";

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
    
    /// <summary>
    /// Parses a command line and creates the respective command.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns>New ICommand command.</returns>
    /// <exception cref="Exception">Command syntax is invalid or could not be parsed.</exception>
    private static ICommand ParseCommand(Queue<ParsedLine> lines)
    {
        var line = lines.Dequeue();
        switch (line.Parts[0])
        {
            case Move: return ParseMoveCommand(line);
            case Turn: return ParseTurnCommand(line);
            case Repeat:
            {
                var times = uint.Parse(line.Parts[1]);
                if (line.Parts[2] != Times)
                    throw new Exception("Malformed syntax");

                List<ICommand> subCommands = [];

                while (lines.Count > 0 && lines.Peek().Indent > line.Indent)
                {
                    subCommands.Add(ParseCommand(lines));
                }

                return CreateRepeat(times, subCommands);
            }
            default: throw new Exception("Malformed syntax");
        }
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
    
    public static MoveCommand CreateMove(uint steps) => new(steps);
    public static TurnCommand CreateTurn(Direction.LeftRight direction) => new(direction);

    public static RepeatCommand CreateRepeat(uint amount, IReadOnlyList<ICommand> subCommands) => new(amount, subCommands);
}