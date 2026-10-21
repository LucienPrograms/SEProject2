namespace SEProject2.Models;

public class Character
{
    public int X { get; private set; }

    public int Y { get; private set; }

    public Direction.Heading Facing { get; private set; }

    private readonly List<string> _executedCommandTrace = [];

    public Character() => ResetToStart();

    public void ResetToStart()
    {
        X = 0;
        Y = 0;
        Facing = Direction.Heading.East;
        _executedCommandTrace.Clear();
    }

    public void MoveForward(uint steps)
    {
        var (dx, dy) = Direction.Step[Facing];
        for (int i = 0; i < steps; i++)
        {
            X = X + dx;
            Y = Y + dy;
        }
        _executedCommandTrace.Add($"Move {steps}");
    }

    public void Turn(Direction.LeftRight direction)
    {
        if (direction == Direction.LeftRight.Left)
        {
            Facing = Direction.Left[Facing];
            _executedCommandTrace.Add("Turn left");
        }
        else
        {
            Facing = Direction.Right[Facing];
            _executedCommandTrace.Add("Turn right");
        }   
    }

    public string GetState() => $"({X}, {Y}) facing {Facing.ToString().ToLower()}";

    public IReadOnlyList<string> GetTraceEntries() => _executedCommandTrace.AsReadOnly();
}