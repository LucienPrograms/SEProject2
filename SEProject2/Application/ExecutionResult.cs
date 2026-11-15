using System;
using System.Collections.Generic;
using System.Text;

namespace SEProject2.Application
{
    /// <summary>
    /// Stores the result of executing a LunarProgram
    /// </summary>
    public class ExecutionResult
    {
        //Gets or sets the trace of commands executed by the character.
        public string Trace { get; set; } = "";

        //Gets or sets the final state of the character after executing the program.
        public string FinalState { get; set; } = "";

        //Gets or sets every position visited by the character, including the starting position.
        public IReadOnlyList<(int X, int Y)> Path { get; set; } = Array.Empty<(int X, int Y)>();
    }
}
