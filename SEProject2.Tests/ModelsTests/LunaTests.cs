namespace SEProject2.Tests.ModelsTests;

using Models;

public class CharacterTests
{
    [Fact]
    public void StartingDirectionTest()
    {
        // Arrange
        Luna luna = new Luna();
            
        // Act
        Direction.Heading heading = luna.Facing;

        // Assert
        Assert.Equal(Direction.Heading.East, heading);
    }

    [Fact]
    public void StartingLocationTest()
    {
        // Arrange
        Luna luna = new Luna();

        // Act
        (int, int) location = (luna.X, luna.Y);

        // Assert
        Assert.Equal((0, 0), location);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(4, 0, 4)]
    [InlineData(10, 0, 10)]
    public void MoveStepsTest(uint steps, int expectedX, int expectedY)
    {
        // Arrange
        Luna luna = new Luna();
        (int, int) expected = (expectedX, expectedY);

        // Act
        luna.MoveForward(steps);
        (int, int) location = (luna.X, luna.Y);

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
        Luna luna = new Luna();

        // Act
        for (int i = 0; i < count; i++)
        {
            luna.Turn(turnDirection);   
        }

        // Assert
        Assert.Equal(expected, luna.Facing);
    }

    [Fact]
    public void ResetCharacterToStartTest()
    {
        // Arrange
        Luna luna = new Luna();
    
        // Act
        luna.MoveForward(4);
        luna.Turn(Direction.LeftRight.Left);
        luna.MoveForward(7);
        luna.Turn(Direction.LeftRight.Right);
        luna.MoveForward(8);

        luna.ResetToStart();

        (int, int) location = (luna.X, luna.Y);
    
        // Assert
        Assert.Equal((0, 0), location);
    }

    [Fact]
    public void ReachGoalTest()
    {
        // Arrange
        Luna luna = new Luna(new Space("oo\n+x"));

        // Act
        luna.MoveForward(1);
        luna.Turn(Direction.LeftRight.Right);
        luna.MoveForward(1);

        //Assert
        Assert.True(luna.GoalReached());
    }

    [Fact]
    public void AsteroidCollisionTest()
    {
        // Arrange
        Luna luna = new Luna(new Space("oo\n+x"));

        // Act
        luna.Turn(Direction.LeftRight.Right);

        //Assert
        Assert.Throws<BlockedLocationException>(() => luna.MoveForward(1));
    }

    [Fact]
    public void LeavingGridTest()
    {
        // Arrange
        Luna luna = new Luna(new Space("oo\n+x"));

        // Act
        luna.Turn(Direction.LeftRight.Left);

        //Assert
        Assert.Throws<OutOfBoundsException>(() => luna.MoveForward(1));
    }
}
