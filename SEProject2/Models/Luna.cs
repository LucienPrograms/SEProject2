namespace SEProject2.Models;

public class Luna
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction.Heading Facing { get; private set; }
    private readonly List<string> _executedCommandTrace = [];

    public Space AsteroidField { get; }

    private readonly List<(int X, int Y)> _path = new();

    /// <summary>
    /// Creates Luna without a loaded asteroid field. 
    /// </summary>
    public Luna() : this(new Space()) {}

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
    /// Creates Luna with an asteroid field in text form.
    /// </summary>
    /// <param name="asteroidFieldText">The asteroid field in text form.</param>
    public Luna(string asteroidFieldText)
    {
        AsteroidField = new Space(asteroidFieldText);
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

    public bool GoalReached() => AsteroidField.IsDestination(X, Y);

    /// <summary>
    /// Checks if the next step forward would enter a blocked space.
    /// </summary>
    /// <returns>Faceplant prospect.</returns>
    
    public bool IsWallAhead()
    {
        try
        {
            (int dx, int  dy) = Direction.Step[Facing];
            AsteroidField.Collision(X + dx, Y + dy);
        }
        catch(BlockedLocationException)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the next step forward would exit the Asteroid Field.
    /// </summary>
    /// <returns>Faceplant prospect.</returns>
    public bool IsGridEdge()
    {
        try
        {
            (int dx, int dy) = Direction.Step[Facing];
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
        (int dx, int dy) = Direction.Step[Facing];

        for (int i = 0; i < steps; i++)
        {
            AsteroidField.Collision(X + dx, Y + dy);
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
    public string GetState() => $"({X}, {Y}) facing {Facing}";

    public IReadOnlyList<string> GetTraceEntries() => _executedCommandTrace.AsReadOnly();

    public IReadOnlyList<(int X, int Y)> GetPath() => _path.AsReadOnly();
}