namespace SEProject2.Tests;
using SEProject2.Models;

public class DirectionTests
{
    [Theory]
    [InlineData(Direction.Heading.North, Direction.Heading.East)]
    [InlineData(Direction.Heading.East, Direction.Heading.South)]
    [InlineData(Direction.Heading.South, Direction.Heading.West)]
    [InlineData(Direction.Heading.West, Direction.Heading.North)]
    public void TestRight(Direction.Heading heading, Direction.Heading expected_heading)
    {
        // arrange
        Direction.Heading new_heading;

        // act
        new_heading = Direction.Right[heading];

        // assert
        Assert.Equal(expected_heading, new_heading);
    }

    [Theory]
    [InlineData(Direction.Heading.North, Direction.Heading.West)]
    [InlineData(Direction.Heading.East, Direction.Heading.North)]
    [InlineData(Direction.Heading.South, Direction.Heading.East)]
    [InlineData(Direction.Heading.West, Direction.Heading.South)]
    public void TestLeft(Direction.Heading heading, Direction.Heading expected_heading)
    {
        // arrange
        Direction.Heading new_heading;

        // act
        new_heading = Direction.Left[heading];

        // ssert
        Assert.Equal(expected_heading, new_heading);
    }
}
