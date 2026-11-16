using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2;

public static class ExamplePrograms
{
    public const string Example1Text = 
        "Move 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right";

    public const string Example2Text = 
        "Repeat 4 times\n    Move 10\n    Turn right";

    public const string Example3Text = 
        "Move 5\nTurn left\nTurn left\nMove 3\nTurn right\nRepeat 3 times\n    Move 1\n    Turn right\n    Repeat 5 times\n        Move 2\nTurn left";

    public static LunarProgram Example1() =>
        ProgramImporter.ImportFromString("Example 1", Example1Text);

    public static LunarProgram Example2() =>
        ProgramImporter.ImportFromString("Example 2", Example2Text);

    public static LunarProgram Example3() =>
        ProgramImporter.ImportFromString("Example 3", Example3Text);
}