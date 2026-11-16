using SEProject2.Domain;
using SEProject2.Models;

namespace SEProject2.Tests.CommandTests;

public class TurnCommandTests
{
    [Fact]
    public void TestTurnLeft()
    {
        // Arrange
        var luna = new Luna();
        var command = new TurnCommand(Direction.LeftRight.Left);

        // Act
        command.Execute(luna);

        // Assert
        Assert.Equal(Direction.Heading.North, luna.Facing);
    }

    [Fact]
    public void TestTurnRight()
    {
        // Arrange
        var luna = new Luna();
        var command = new TurnCommand(Direction.LeftRight.Right);

        // Act
        command.Execute(luna);

        // Assert
        Assert.Equal(Direction.Heading.South, luna.Facing);
    }
}