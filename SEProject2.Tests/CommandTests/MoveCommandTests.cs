using SEProject2.Domain;
using SEProject2.Models;

namespace SEProject2.Tests.CommandTests;

public class MoveCommandTests
{
    [Fact]
    public void MoveZero()
    {
        // Arrange
        var space = new Space("oooo+\n+++o+\n+x+o+\n+ooo+\n+++++");

        var luna = new Luna(space);
        var command = new MoveCommand(0);
        var oldPos = (luna.X, luna.Y);
        
        // Act
        command.Execute(luna);

        // Assert
        Assert.Equal(oldPos, (luna.X, luna.Y));
    }
}