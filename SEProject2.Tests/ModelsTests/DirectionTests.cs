using SEProject2.Models;

namespace SEProject2.Tests.ModelsTests;

public class DirectionTests
{
    [Theory]
    [InlineData(Direction.Heading.North, Direction.Heading.East)]
    [InlineData(Direction.Heading.East, Direction.Heading.South)]
    [InlineData(Direction.Heading.South, Direction.Heading.West)]
    [InlineData(Direction.Heading.West, Direction.Heading.North)]
    public void TestRight(Direction.Heading heading, Direction.Heading expectedHeading)
    {
        // Arrange
        Direction.Heading newHeading = Direction.Right[heading];

        // Assert
        Assert.Equal(expectedHeading, newHeading);
    }

    [Theory]
    [InlineData(Direction.Heading.North, Direction.Heading.West)]
    [InlineData(Direction.Heading.East, Direction.Heading.North)]
    [InlineData(Direction.Heading.South, Direction.Heading.East)]
    [InlineData(Direction.Heading.West, Direction.Heading.South)]
    public void TestLeft(Direction.Heading heading, Direction.Heading expectedHeading)
    {
        // Arrange
        Direction.Heading newHeading = Direction.Left[heading];

        // Assert
        Assert.Equal(expectedHeading, newHeading);
    }

    [Theory]
    [InlineData(Direction.Heading.North, -1, 0)]
    [InlineData(Direction.Heading.East, 0, 1)]
    [InlineData(Direction.Heading.South, 1, 0)]
    [InlineData(Direction.Heading.West, 0, -1)]
    public void TestStep(Direction.Heading heading, int expectedDeltaX, int expectedDeltaY)
    {
        // Arrange
        (int, int) delta = Direction.Step[heading];
        (int, int) expected = (expectedDeltaX, expectedDeltaY);

        // Assert
        Assert.Equal(expected, delta);
    }
}
