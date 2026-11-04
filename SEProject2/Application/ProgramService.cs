using System;
using System.Collections.Generic;
using System.Text;
using SEProject2.Domain;
using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2.Application;

public class ProgramService
{
    private UserProgram? currentProgram;

    public void LoadProgram(string filePath)
    {
        currentProgram = ProgramImporter.ImportFromFile(filePath);
    }

    public void LoadProgramFromText(string name, string text)
    {
        string[] lines = text.Replace("\r\n", "\n").Split('\n');
        var parsedLines = LineParser.ParseBlock(lines);

        var commands = CommandFactory.FromLines(parsedLines);

        currentProgram = new UserProgram(name, commands);
    }

    public ExecutionResult RunProgram()
    {
        if (currentProgram == null)
        {
            throw new InvalidOperationException("No program loaded.");
        }
        currentProgram.Execute();

        return new ExecutionResult
        {
            Trace = string.Join(", ", currentProgram.Character.GetTraceEntries()), 
            FinalState = currentProgram.Character.GetState()
        };
    }

    public ProgramMetrics GetMetrics()
    {
        if (currentProgram == null)
        {
            throw new InvalidOperationException("No program loaded.");
        }
        return currentProgram.CalculateMetrics();
    }

}
