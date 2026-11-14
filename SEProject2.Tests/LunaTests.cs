namespace SEProject2.Tests;

using SEProject2.Models;

public class CharacterTests
{
    [Fact]
    public void StartingDirectionTest()
    {
        // Arrange
        Luna Luna = new Luna();
            
        // Act
        Direction.Heading heading = Luna.Facing;

        // Assert
        Assert.Equal(Direction.Heading.East, heading);
    }

    [Fact]
    public void StartingLocationTest()
    {
        // Arrange
        Luna Luna = new Luna();

        // Act
        (int, int) location = (Luna.X, Luna.Y);

        // Assert
        Assert.Equal((0, 0), location);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(4, 4, 0)]
    [InlineData(10, 10, 0)]
    public void MoveStepsTest(uint steps, int expected_x, int expected_y)
    {
        // Arrange
        Luna Luna = new Luna();
        (int, int) expected = (expected_x, expected_y);

        // Act
        Luna.MoveForward(steps);
        (int, int) location = (Luna.X, Luna.Y);

        // Assert
        Assert.Equal(expected, location);
    }

    [Theory]
    [InlineData(Direction.LeftRight.Left, 0, Direction.Heading.East)]
    [InlineData(Direction.LeftRight.Left, 1, Direction.Heading.North)]
    [InlineData(Direction.LeftRight.Left, 2, Direction.Heading.West)]
    [InlineData(Direction.LeftRight.Left, 3, Direction.Heading.South)]
    [InlineData(Direction.LeftRight.Right, 0, Direction.Heading.East)]
    [InlineData(Direction.LeftRight.Right, 1, Direction.Heading.South)]
    [InlineData(Direction.LeftRight.Right, 2, Direction.Heading.West)]
    [InlineData(Direction.LeftRight.Right, 3, Direction.Heading.North)]
    public void TurnCharacterTest(Direction.LeftRight turnDirection, int count, Direction.Heading expected)
    {
        // Arrange
        Luna Luna = new Luna();

        // Act
        for (int i = 0; i < count; i++)
        {
            Luna.Turn(turnDirection);   
        }

        // Assert
        Assert.Equal(expected, Luna.Facing);
    }

    [Fact]
    public void ResetCharacterToStartTest()
    {
        // Arrange
        Luna Luna = new Luna();
    
        // Act
        Luna.MoveForward(4);
        Luna.Turn(Direction.LeftRight.Left);
        Luna.MoveForward(7);
        Luna.Turn(Direction.LeftRight.Right);
        Luna.MoveForward(8);

        Luna.ResetToStart();

        (int, int) location = (Luna.X, Luna.Y);
    
        // Assert
        Assert.Equal((0, 0), location);
    }
}
