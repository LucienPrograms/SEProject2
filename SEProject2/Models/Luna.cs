namespace SEProject2.Models;

public class Luna
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction.Heading Facing { get; private set; }
    private readonly List<string> _executedCommandTrace = [];
    public Space AsteroidField { get; set; } = new Space("");

    private readonly List<(int X, int Y)> _path = new();

    /// <summary>
    /// Creates Luna without a loaded asteroid field. 
    /// </summary>
    public Luna()
    {
        AsteroidField = new Space("");
        ResetToStart();
    }

    /// <summary>
    /// Creates Luna with the supplied asteroid field. 
    /// </summary>
    /// <param name="asteroidField">The grid used for collision detection</param> 
    public Luna(Space asteroidField)
    {
        AsteroidField = asteroidField;
        ResetToStart();
    }



    /// <summary>
    /// (Re)sets Luna to starting position and direction. (0, 0) facing East.
    /// </summary>
    public void ResetToStart()
    {
        X = 0;
        Y = 0;
        Facing = Direction.Heading.East;

        _executedCommandTrace.Clear();

        _path.Clear();
        _path.Add((X, Y));

    }

    public bool IsWallAhead()
    {
        try
        {
            var (dx, dy) = Direction.Step[Facing];
            AsteroidField.Collision(X + dx, Y + dy);
        }
        catch(BlockedLocationException)
        {
            return true;
        }
        return false;
    }

    public bool IsGridEdge()
    {
        try
        {
            var (dx, dy) = Direction.Step[Facing];
            AsteroidField.Collision(X + dx, Y + dy);
        }
        catch(OutOfBoundsException)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Moves Luna in the direction she is facing.
    /// </summary>
    /// <param name="steps">How many steps Luna is taking.</param>
    public void MoveForward(uint steps)
    {
        var (dx, dy) = Direction.Step[Facing];

        AsteroidField.Collision(X + dx, Y + dy);

        for (int i = 0; i < steps; i++)
        {
            X += dx;
            Y += dy;

            //Record every position visited
            _path.Add((X, Y));
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

    public IReadOnlyList<(int X, int Y)> GetPath() => _path.AsReadOnly();
}