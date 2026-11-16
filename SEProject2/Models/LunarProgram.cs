using SEProject2.Domain;
using SEProject2.Service;

namespace SEProject2.Models;

public class LunarProgram(string name, List<ICommand> commands, Space? asteroidField = null)
{
    public string Name { get; set; } = name;
    public List<ICommand> Commands { get; set; } = commands;
    public Luna Character { get; private set; } = 
        asteroidField == null? new Luna() : new Luna(asteroidField);

    /// <summary>
    /// Runs the LunarProgram by executing the stored list of commands in order.
    /// </summary>
    public void Execute()
    {
        Character.ResetToStart();  // Always reset to start before executing

        foreach (var command in Commands)
        {
            command.Execute(Character);
        }
    }
}