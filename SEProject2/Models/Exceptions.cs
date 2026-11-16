namespace SEProject2.Models;

public class OutOfBoundsException(string message) : Exception(message);

public class BlockedLocationException(string message) : Exception(message);

public class InvalidSpaceException(string message) : Exception(message);
