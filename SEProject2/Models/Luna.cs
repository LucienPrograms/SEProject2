namespace SEProject2.Models;

public class Luna
{
    public int X { get; private set; }

    public int Y { get; private set; }

    public Direction.Heading Facing { get; private set; }

    private readonly List<string> _executedCommandTrace = [];

    public Luna() => ResetToStart();

    /// <summary>
    /// (Re)sets Luna to starting position and direction. (0, 0) facing East.
    /// </summary>
    public void ResetToStart()
    {
        X = 0;
        Y = 0;
        Facing = Direction.Heading.East;
        _executedCommandTrace.Clear();
    }

    /// <summary>
    /// Moves Luna in the direction she is facing.
    /// </summary>
    /// <param name="steps">How many steps Luna is taking.</param>
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

    /// <summary>
    /// Changes the direction Luna is facing by 90° either to the right (clockwise) or to the left (counterclockwise).
    /// </summary>
    /// <param name="direction">Direction in which to turn.</param>
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

    /// <summary>
    /// Retrieves the current position and direction of Luna.
    /// </summary>
    /// <returns>string with location (X, Y) and the direction Luna is facing (Direction.Heading).</returns>
    public string GetState() => $"({X}, {Y}) facing {Facing.ToString().ToLower()}";

    public IReadOnlyList<string> GetTraceEntries() => _executedCommandTrace.AsReadOnly();
}