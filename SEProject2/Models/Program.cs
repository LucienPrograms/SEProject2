using SEProject2.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEProject2.Models
{
    public class Program
    {
        public string Name { get; set; } = string.Empty;
        public List<Command> Commands { get; set; } = new();
        public Character Character { get; set; } = new();

        public Program(string name, List<Command> commands)
        {
            Name = name;
            Commands = commands;
            Character = new Character();
        }

        public void Execute()
        {
            Character.ResetToStart();  // Always reset to start before executing
            foreach (Command command in Commands)
            {
                command.Execute(Character);
            }

            string commandTrace = string.Join(", ", Character.GetTraceEntries()) + ".";
            Console.WriteLine(commandTrace);
            Console.WriteLine($"End state {Character.GetState()}.");
        }

        public ProgramMetrics CalculateMetrics()
        {
            var metrics = new ProgramMetrics();
            //metrics.Analyze(this);
            return metrics;
        }
    
    }
}
