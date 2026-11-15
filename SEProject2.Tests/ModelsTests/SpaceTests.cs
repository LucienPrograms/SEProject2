using SEProject2.Models;

namespace SEProject2.Tests.ModelsTests;

public class SpaceTests
{
    [Theory]
    [InlineData("oo+++\n+oo++\n++o++\n++o++\n++oox", 5, 5)]
    [InlineData("o+x\no+o\no+o\no+o\no+o\no+o\nooo", 7, 3)]
    [InlineData("ooox", 1, 4)]
    public void SizeTest(string asteroidFieldText, int expectedHeight, int expectedWidth)
    {
        // arrange
        Space asteroidField = new Space(asteroidFieldText);

        // act
        int height = asteroidField.Height;
        int width = asteroidField.Width;

        // assert
        Assert.Equal(expectedHeight, height);
        Assert.Equal(expectedWidth, width);
    }

    [Fact]
    public void AsteroidCollisionTest()
    {
        // arrange
        Space asteroidField = new Space("oo\n+x");

        // assert
        Assert.Throws<BlockedLocationException>(() => asteroidField.Collision(1, 0));
    }

    [Theory]
    [InlineData("oo\n+o\nox", -1, 0)]
    [InlineData("oo\n+o\nox", 0, -1)]
    [InlineData("oo\n+o\nox", 3, 0)]
    [InlineData("oo\n+o\nox", 0, 2)]
    public void OutOfBoundsCollisionTest(string asteroidFieldText, int xCoordinate, int yCoordinate)
    {
        // arrange
        Space asteroidField = new Space(asteroidFieldText);

        // assert
        Assert.Throws<OutOfBoundsException>(() => asteroidField.Collision(xCoordinate, yCoordinate));
    }

    [Fact]
    public void IsDestinationTest()

    {
        // arrange
        Space asteroidField = new Space("oo\n+o\n+x");

        // assert
        Assert.False(asteroidField.IsDestination(0, 0));
        Assert.False(asteroidField.IsDestination(1, 1));
        Assert.False(asteroidField.IsDestination(1, 0));
        Assert.True(asteroidField.IsDestination(2, 1));
    }

    [Theory]
    [InlineData("oo+++\n+oo++\n++o++\n++o+++\n++oox")]
    [InlineData("oo+++\n+oo+\n++o++\n++o++\n++oox")]
    public void InvalidSpaceSizeTest(string asteroidFieldText)
    {
        // assert
        Exception exception = Assert.Throws<InvalidSpaceException>(() => new Space(asteroidFieldText));

        // verify reason
        Assert.Equal("Line lengths do not match.", exception.Message);
    }

    [Theory]
    [InlineData("oo\n-o\n-x")]
    [InlineData("00\n-0\n-x")]
    [InlineData("abc\ndef\nghi")]
    public void InvalidSpaceCharacterTest(string asteroidFieldText)
    {
        // assert
        Exception exception = Assert.Throws<InvalidSpaceException>(() => new Space(asteroidFieldText));

        // verify reason
        Assert.Equal("Invalid character.", exception.Message);
    }

    [Fact]
    public void EmptySpaceCollisionTest()
    {
        // arrange
        Space asteroidField = new Space();

        // act
        Exception? exception = Record.Exception(() => asteroidField.Collision(0, 0));
            
        // assert
        Assert.Null(exception);
    }

    [Fact]
    public void EmptySpaceDestinationTest()
    {
        // arrange
        Space asteroidField = new Space();

        // act
        Exception? exception = Record.Exception(() => asteroidField.IsDestination(0, 0));
            
        // assert
        Assert.Null(exception);
        Assert.True(asteroidField.IsDestination(0, 0));
    }
}