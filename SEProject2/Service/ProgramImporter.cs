using SEProject2.Domain;
using SEProject2.Models;

namespace SEProject2.Service;

public static class ProgramImporter
{
    public static LunarProgram ImportFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
    
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file '{filePath}' does not exist.");
    
        var lines = File.ReadAllLines(filePath);
    
        return new LunarProgram(
            Path.GetFileNameWithoutExtension(filePath), CommandFactory.FromLines(LineParser.ParseBlock(lines))
        );
    }

    public static LunarProgram ImportFromString(string programName, string program) => 
        new(programName, CommandFactory.FromLines(LineParser.ParseBlock(program.Split('\n'))));
    
    private const string Example1Text = 
        "Move 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right\nMove 10\nTurn right";
    private const string Example2Text = 
        "Repeat 4 times\n    Move 10\n    Turn right";
    private const string Example3Text = 
        "Move 5\nTurn left\nTurn left\nMove 3\nTurn right\nRepeat 3 times\n    Move 1\n    Turn right\n    Repeat 5 times\n        Move 2\nTurn left";

    public static LunarProgram Example1() =>
        ImportFromString("Example 1", Example1Text);
    public static LunarProgram Example2() =>
        ImportFromString("Example 2", Example2Text);
    public static LunarProgram Example3() =>
        ImportFromString("Example 3", Example3Text);
}