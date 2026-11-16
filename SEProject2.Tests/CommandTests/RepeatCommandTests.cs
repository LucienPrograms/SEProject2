using SEProject2.Domain;
using SEProject2.Models;

namespace SEProject2.Tests.CommandTests;

public class RepeatCommandTests
{
    [Fact]
    public void TestRepeatsCorrectly()
    {
        // Arrange
        var luna = new Luna();

        var commands = new List<ICommand>
        {
            new TurnCommand(Direction.LeftRight.Right),
        };

        var command = new RepeatCommand(4, commands);
        var oldFacing = luna.Facing;
        
        // Act
        command.Execute(luna);

        // Assert
        Assert.Equal(luna.Facing, oldFacing); // 4x right is old facing
    }

    [Fact]
    public void TestRepeatZeroTimes()
    {
        // Arrange
        var luna = new Luna();
        var command = new RepeatCommand(0, new List<ICommand>
        {
            new MoveCommand(2)
        });

        // Act
        command.Execute(luna);

        // Assert
        Assert.Equal(0, luna.X);
        Assert.Equal(0, luna.Y);
    }
}