namespace SEProject2.Tests;
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
}
