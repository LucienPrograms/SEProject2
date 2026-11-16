using SEProject2.Models;

namespace SEProject2.Domain;

public enum RepeatCondition { WallAhead, GridEdge }

public class RepeatUntilCommand(RepeatCondition condition, IReadOnlyList<ICommand> subCommands) : ICommand
{
    public IReadOnlyList<ICommand> SubCommands { get; } = subCommands;

    private bool ConditionIsNotMet(Luna c) => !(condition switch
    { 
        RepeatCondition.GridEdge => c.IsGridEdge(), 
        RepeatCondition.WallAhead => c.IsWallAhead()
    });

    public void Execute(Luna c)
    {
        while (ConditionIsNotMet(c))
        {
            foreach (var subCommand in SubCommands)
            {
                subCommand.Execute(c);
            }
        }
    }
}