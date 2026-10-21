using SEProject2.Models;

namespace SEProject2.Domain;

public class MoveCommand(uint steps) : ICommand
{
    public void Execute(Character c) => c.MoveForward(steps);
}