using SEProject2.Domain;
using SEProject2.Models;
using SEProject2.Service;

namespace SEProject2.Tests.ModelsTests;

public class MetricsTests
{
    [Fact]
    public void TestEmptyProgram()
    {
        // Arrange
        const string text = "";
        var program = ProgramImporter.ImportFromString("Test zero", text);

        // Act
        var result = ProgramMetrics.FromProgram(program);

        // Assert
        Assert.Equal(0, result.MaxNesting);
        Assert.Equal(0, result.CommandCount);
        Assert.Equal(0, result.RepeatCommandCount);
    }

    [Fact]
    public void TestSingleRepeat()
    {
        // Arrange
        const string text = "Repeat 4 times\n    Turn left";
        var program = ProgramImporter.ImportFromString("Test single repeat", text);
        
        // Act
        var result = ProgramMetrics.FromProgram(program);

        // Assert
        Assert.Equal(1, result.MaxNesting);
        Assert.Equal(2, result.CommandCount);
        Assert.Equal(1, result.RepeatCommandCount);
    }

    [Fact]
    public void TestNestedRepeats()
    {
        // Arrange
        const string x = "Repeat 2 times\n    Repeat 3 times\n    RepeatUntil WallAhead\n    Turn left";
        var program = ProgramImporter.ImportFromString("Test nested repeat", x);

        // Act
        var result = ProgramMetrics.FromProgram(program);

        // Assert
        Assert.Equal(3, result.MaxNesting);
        Assert.Equal(4, result.CommandCount);
        Assert.Equal(3, result.RepeatCommandCount);
    }

    [Fact]
    public void TestMultipleTopLevelRepeats()
    {
        // Arrange
        const string text = "Repeat 4 times\n    Turn left\nRepeatUntil GridEdge\n    Turn right";
        var program = ProgramImporter.ImportFromString("Test multiple repeat", text);

        // Act
        var result = ProgramMetrics.FromProgram(program);

        // Assert
        Assert.Equal(1, result.MaxNesting);
        Assert.Equal(4, result.CommandCount);
        Assert.Equal(2, result.RepeatCommandCount);
    }
}