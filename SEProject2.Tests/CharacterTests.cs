namespace SEProject2.Tests;

using System.Globalization;
using SEProject2.Models;
using SEProject2.Tests;

public class CharacterTests
{
    [Fact]
    public void StartingDirectionTest()
    {
        //arrange
        Character Luna = new Character();
            
        //act
        Direction.Heading heading = Luna.Facing;

        //assert
        Assert.Equal(Direction.Heading.EAST, heading);
    }

    [Fact]
    public void StartingLocationTest()
    {
        //arrange
        Character Luna = new Character();

        //act
        (int, int) location = (Luna.X, Luna.Y);

        //assert
        Assert.Equal((0, 0), location);
    }

    [Theory]
    [InlineData(10, (10, 0))]
    [InlineData(4, (4, 0))]
    public void MoveStepsTest(int steps, (int, int) expected)
    {
        //arrange
        Character Luna = new Character();

        //act
        Luna.MoveForward(steps);
        (int, int) location = (Luna.X, Luna.Y);

        //assert
        Assert.Equal(expected, location);
    }

    [Theory]
    ()
    public void MoveStepsErrorTest(int steps, )
    {
        //arrange
        Character Luna = new Character();

        //act
        Action action = () => Luna.MoveForward()

        //assert
        ArgumentException exception = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Unknown direction", exception);
    }

    [Theory]
    public void TurnCharacterTest()
    {
        
    }
}
