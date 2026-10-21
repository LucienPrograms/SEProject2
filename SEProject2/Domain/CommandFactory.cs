using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2.Domain;

public static class CommandFactory
{
    private const string Move = "Move";
    private const string Turn = "Turn";
    private const string Repeat = "Repeat";
    private const string Right = "right";
    private const string Left = "left";
    private const string Times = "times";

    private static MoveCommand ParseMoveCommand(ParsedLine line)
    {
        var steps = uint.Parse(line.Parts[1]);
        return CreateMove(steps);
    }

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
    
    
    private static ICommand ParseCommand(Queue<ParsedLine> lines, uint currentIndent)
    {
        var line = lines.Dequeue();
        switch (line.Parts[0])
        {
            case Move: return ParseMoveCommand(line);
            case Turn: return ParseTurnCommand(line);
            case Repeat:
            {
                var times = uint.Parse(line.Parts[1]);
                if (line.Parts[2] != Times) throw new Exception("Malformed syntax");

                List<ICommand> subCommands = [];
            
                var next = lines.Peek();
                while (next.Indent > currentIndent)
                {
                    subCommands.Add(ParseCommand(lines, currentIndent + 1));
                
                    next = lines.Peek();
                }
                return CreateRepeat(times, subCommands);
            }
            default: throw new Exception("Malformed syntax");
        }
    }

    public static List<ICommand> FromLines(IReadOnlyList<ParsedLine> lines)
    {
        var commands = new List<ICommand>();
        var queue = new Queue<ParsedLine>(lines);
        
        while (queue.Count > 0)
        {
            commands.Add(ParseCommand(queue, 0));
        }
        
        return commands;
    }
    
    public static MoveCommand CreateMove(uint steps) => new(steps);
    public static TurnCommand CreateTurn(Direction.LeftRight direction) => new(direction);

    public static RepeatCommand CreateRepeat(uint amount, IReadOnlyList<ICommand> subCommands) => new(amount, subCommands);
}