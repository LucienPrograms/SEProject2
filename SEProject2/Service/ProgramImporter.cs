using SEProject2.Domain;

namespace SEProject2.Service;

public static class ProgramImporter
{
    public static Models.UserProgram ImportFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }
    
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file '{filePath}' does not exist.");
        }
    
        string[] lines = File.ReadAllLines(filePath);
    
        return new Models.UserProgram(
    
            Path.GetFileNameWithoutExtension(filePath), CommandFactory.FromLines(LineParser.ParseBlock(lines))
        );
    }   
}