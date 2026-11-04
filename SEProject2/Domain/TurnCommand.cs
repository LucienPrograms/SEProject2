using SEProject2.Models;

namespace SEProject2.Domain;

public class TurnCommand(Direction.LeftRight direction) : ICommand
{
    public void Execute(Luna c) => c.Turn(direction);
}