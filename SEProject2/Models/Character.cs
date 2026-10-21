namespace SEProject2.Models;

public class Character
{
    public int X { get; private set; }

    public int Y { get; private set; }

    public Direction.Heading Facing { get; private set; }

    private readonly List <String> executedCommandTrace = new List<string>();

    public Character()
    {
        ResetToStart();
    }

    public void ResetToStart()
    {
        X = 0;
        Y = 0;
        Facing = Direction.Heading.East;
        executedCommandTrace.Clear();
    }

    public void MoveForward(uint steps)
    {
        try
        {   
            int x, y;
            (x, y) = Direction.Step[Facing];
            for (int i = 0; i < steps; i++)
            {
                X = X + x;
                Y = Y + y;
            }
            executedCommandTrace.Add($"Move {steps}");
        }
        catch
        {
            throw new InvalidOperationException("Unknown direction");
        }
    }

    public void Turn(Direction.LeftRight direction)
    {
        try
        {
            switch (direction)
            {                
                case Direction.LeftRight.Left:
                    Facing = Direction.Left[Facing];
                    executedCommandTrace.Add("Turn left");
                    break;
                case Direction.LeftRight.Right:
                    Facing = Direction.Right[Facing];
                    executedCommandTrace.Add("Turn right");
                    break;
            }                
        }
        catch
        {
            throw new InvalidOperationException("Unknown direction");
        }
    }

    public string GetState()
    {
        return $"({X}, {Y}) facing {Facing.ToString().ToLower()}";
    }

    public IReadOnlyList<string> GetTraceEntries()
    {
        return executedCommandTrace.AsReadOnly();
    }
}