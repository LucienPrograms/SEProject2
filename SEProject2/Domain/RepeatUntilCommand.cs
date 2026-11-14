using SEProject2.Models;

namespace SEProject2.Domain;

public class RepeatUntilCommand(RepeatCondition condition, IReadOnlyList<ICommand> subCommands) : ICommand
{
    public IReadOnlyList<ICommand> SubCommands { get; } = subCommands;

    private bool CheckCondition(Luna c) => condition switch
    { 
        RepeatCondition.GridEdge => c.IsGridEdge(), 
        RepeatCondition.WallAhead => c.IsWallAhead()
    };

    public void Execute(Luna c)
    {
        bool conditionNotMet = CheckCondition(c);
        while (conditionNotMet)
        {
            foreach (var subCommand in SubCommands)
            {
                subCommand.Execute(c);
            }

            conditionNotMet = CheckCondition(c);
        }
    }
}