using SEProject2.Models;

namespace SEProject2.Domain;

public record MoveCommand(uint steps) : ICommand
{
    public void Execute(Character c) => c.MoveForward(steps);
}