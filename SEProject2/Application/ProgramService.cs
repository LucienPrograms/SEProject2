using System;
using System.Collections.Generic;
using System.Text;
using SEProject2.Domain;
using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2.Application;


/// <summary>
/// Provides the application logic for loading grids and programs,
/// executing programs and retrieving program metrics
/// </summary>
public class ProgramService
{
    private Space? currentSpace;
    private LunarProgram? currentProgram;

    /// <summary>
    /// Loads the grid text and stores it for the next program execution. 
    /// </summary>
    /// <param name="gridText"> The contents of the grid file </param>
    public void LoadGridFromText(string gridText)
    {
        currentSpace = new Space(gridText);
    }

    /// <summary>
    /// Parses program text and creates a program using the currently loaded grid
    /// </summary>
    /// <param name="name"> The name of the program</param>
    /// <param name="text"> The program instructions as text </param>
    /// <exception cref="InvalidOperationException"> Throw when no grid has been loaded </exception>
    public void LoadProgramFromText(string name, string text)
    {
        string[] lines = text.Replace("\r\n", "\n").Replace("\t", "  ").Split('\n');
        var parsedLines = LineParser.ParseBlock(lines);

        var commands = CommandFactory.FromLines(parsedLines);

        if (currentSpace == null)
        {
            throw new InvalidOperationException("No grid loaded.");
        }

        currentProgram = new LunarProgram(name, commands, currentSpace);
    }

    /// <summary>
    /// Executes the loaded program and returns its execution result
    /// The result also contains the path and error information if 
    /// a runtime error occurs
    /// </summary>
    /// <returns> The execution trace, final state and character path </returns>
    /// <exception cref="InvalidOperationException"> Throw when no program has been loaded </exception>

    public ExecutionResult RunProgram()
    {

        if (currentProgram == null)
        {
            throw new InvalidOperationException("No program loaded.");
        }

        string? runtimeError = null;

        try
        {
            currentProgram.Execute();
        }
        catch (Exception ex)
        {
            runtimeError = ex.Message;
        }
       

        return new ExecutionResult
        {
            Trace = string.Join(", ", currentProgram.Character.GetTraceEntries()), 
            FinalState = currentProgram.Character.GetState(),
            Path = currentProgram.Character.GetPath(),
            GoalReached = runtimeError == null && 
                currentProgram.Character.GoalReached(),
            RuntimeError = runtimeError
        };
    }

    /// <summary>
    /// Retrieves metrics for the currently loaded program
    /// </summary>
    /// <returns> A record containing the command count, maximum nesting depth, and repeat count </returns>
    /// <exception cref="InvalidOperationException"> Throw when no program has been loaded </exception>
    public ProgramMetrics GetMetrics() =>
        currentProgram == null
            ? throw new InvalidOperationException("No program loaded.")
            : ProgramMetrics.FromProgram(currentProgram);
}
