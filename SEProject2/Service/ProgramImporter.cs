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
}