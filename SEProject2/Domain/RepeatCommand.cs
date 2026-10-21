using SEProject2.Models;

namespace SEProject2.Domain;

public record RepeatCommand(uint amount, IReadOnlyList<ICommand> subCommands) : ICommand
{
    public IReadOnlyList<ICommand> SubCommands { get; } = subCommands;
    
    public void Execute(Character c)
    {
        for (uint i = 0; i < amount; i++)
        {
            foreach (var subCommand in SubCommands)
            {
                subCommand.Execute(c);
            }
        }
    }
}