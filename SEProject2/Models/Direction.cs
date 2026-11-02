namespace SEProject2.Models;

public static class Direction
{
    public enum Heading
    {
        East,
        West,
        North,
        South
    }
    public enum LeftRight
    {
        Left,
        Right
    }
    public static readonly Dictionary<Heading, Heading> Right = new()
    {
        {Heading.North, Heading.East},
        {Heading.East, Heading.South},
        {Heading.South, Heading.West},
        {Heading.West, Heading.North}
    };
    public static readonly Dictionary<Heading, Heading> Left = new()
    {
        {Heading.North, Heading.West},
        {Heading.East, Heading.North},
        {Heading.South, Heading.East},
        {Heading.West, Heading.South}
    };
    public static readonly Dictionary<Heading, (int, int)> Step = new()
    {
        {Heading.North, (0, 1)},
        {Heading.East, (1, 0)},
        {Heading.South, (0, -1)},
        {Heading.West, (-1, 0)}
    };
}