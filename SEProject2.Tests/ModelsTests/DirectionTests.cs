namespace SEProject2.Tests;
using SEProject2.Models;

public class DirectionTests
{
    [Theory]
    [InlineData(Direction.Heading.NORTH, Direction.Heading.EAST)]
    [InlineData(Direction.Heading.EAST, Direction.Heading.SOUTH)]
    [InlineData(Direction.Heading.SOUTH, Direction.Heading.WEST)]
    [InlineData(Direction.Heading.WEST, Direction.Heading.NORTH)]
    public void TestRight(Direction.Heading heading, Direction.Heading expected_heading)
    {
        //arrange
        Direction.Heading new_heading;

        //act
        new_heading = Direction.Right[heading];

        //assert
        Assert.Equal(expected_heading, new_heading);
    }

    [Theory]
    [InlineData(Direction.Heading.NORTH, Direction.Heading.WEST)]
    [InlineData(Direction.Heading.EAST, Direction.Heading.NORTH)]
    [InlineData(Direction.Heading.SOUTH, Direction.Heading.EAST)]
    [InlineData(Direction.Heading.WEST, Direction.Heading.SOUTH)]
    public void TestLeft(Direction.Heading heading, Direction.Heading expected_heading)
    {
                //arrange
        Direction.Heading new_heading;

        //act
        new_heading = Direction.Left[heading];

        //assert
        Assert.Equal(expected_heading, new_heading);
    }
}
