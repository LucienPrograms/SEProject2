namespace SEProject2.Models;

public class OutOfBoundsException : Exception
{
    public OutOfBoundsException(string message) : base(message) { }

}

public class BlockedLocationException : Exception
{
    public BlockedLocationException(string message) : base(message) { }
}

public class InvalidSpaceException : Exception
{
    public InvalidSpaceException(string message) : base(message) { }
}