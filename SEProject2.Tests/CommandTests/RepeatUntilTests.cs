using SEProject2.Domain;
using SEProject2.Models;

namespace SEProject2.Tests.CommandTests;

public class RepeatUntilTests
{
    [Fact]
    public void TestWallAhead()
    {
        // Arrange
        var space = new Space(
            "o++++"
        );

        var luna = new Luna(space);
        var oldFacing = luna.Facing;

        var commands = new List<ICommand>
        {
            new TurnCommand(Direction.LeftRight.Right)
        };

        var command = new RepeatUntilCommand(
            RepeatCondition.WallAhead,
            commands);

        // Act
        command.Execute(luna);

        // Assert
        Assert.Equal(oldFacing, luna.Facing);
    }

    [Fact]
    public void TestGridEdge()
    {
        // Arrange
        var space = new Space(
            "o"
        );

        var luna = new Luna(space);
        var oldFacing = luna.Facing;

        var commands = new List<ICommand>
        {
            new TurnCommand(Direction.LeftRight.Right)
        };

        var command = new RepeatUntilCommand(
            RepeatCondition.GridEdge,
            commands);

        // Act
        command.Execute(luna);

        // Assert
        Assert.Equal(oldFacing, luna.Facing);
    }
}