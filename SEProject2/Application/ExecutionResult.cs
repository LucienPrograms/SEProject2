using System;
using System.Collections.Generic;
using System.Text;

namespace SEProject2.Application
{
    public class ExecutionResult
    {
        public string Trace { get; set; } = "";
        public string FinalState { get; set; } = "";

        //Every position visited by the character, including the start.
        public IReadOnlyList<(int X, int Y)> Path { get; set; } = Array.Empty<(int X, int Y)>();
    }
}
