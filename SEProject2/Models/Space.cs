namespace SEProject2.Models;

public class Space
{
    public int Height { get; }
    public int Width { get; }
    List<List<States>> AsteroidField { get; } = [];

    private enum States { Asteroid, Open, End }

    /// <summary>
    /// Creates a new Space object denoting the existence of an Asteroid Field.
    /// </summary>
    /// <param name="asteroidFieldText">Asteroid field in text form.</param>
    /// <exception cref="Exception">Thrown when provided Asteroid Field is not rectangular.</exception>
    public Space(string asteroidFieldText = "")
    {
        string[] lines = asteroidFieldText.Split("\n");

        if (!CheckLineLength(lines)) throw new InvalidSpaceException("Line lengths do not match.");

        Height = lines.Length; Width = lines[0].Length;

        if (asteroidFieldText.Length > 0) ParseGrid(lines);
    }

    /// <summary>
    /// Parses an Asteroid field in text form to populate the Asteroid Field with Asteroids.
    /// </summary>
    /// <param name="lines"></param>
    private void ParseGrid(string[] lines)
    {
        int row = 0;
        foreach (string line in lines)
        {
            AsteroidField.Add([]);
            foreach (char character in line) AsteroidField[row].Add(ConvertToState(character));
            row++;
        }
    }

    /// <summary>
    /// Converts a grid value into its respective State.
    /// </summary>
    /// <param name="character">Ascii character to convert.</param>
    /// <returns></returns>
    /// <exception cref="Exception">Thrown when the provided Ascii character is not one of the possible grid values.</exception>
    private States ConvertToState(char character) => character switch
    {
        '+' => States.Asteroid,
        'o' => States.Open,
        'x' => States.End,
        _ => throw new InvalidSpaceException("Invalid character.")
    };

    /// <summary>
    /// Checks if an array is of rectangular form.
    /// </summary>
    /// <param name="lines">List of lines to compare lengths of.</param>
    /// <returns>true/false</returns>
    private bool CheckLineLength(string[] lines)
    {
        int length = lines[0].Length;
        foreach (string line in lines.Skip(1))
        {
            if (line.Length != length) return false;
        }
        return true;
    }

    /// <summary>
    /// Throws an exception when a collision is ascertained. Does nothing if no Asteroid Field exists or the provided location can be safely moved to.
    /// </summary>
    /// <param name="x">Vertical coordinate.</param>
    /// <param name="y">Horizontal coordinate.</param>
    /// <exception cref="OutOfBoundsException">Thrown when provided location exists outside of the defined limits of the Asteroid Field.</exception>
    /// <exception cref="BlockedLocationException">Thrown when provided location is an Asteroid.</exception>
    public void Collision (int x, int y)
    {
        if (!AsteroidField.Any()) 
            return;
        if (x < 0 || x >= Height || y < 0 || y >= Width) 
            throw new OutOfBoundsException($" Position ({x}, {y}) is outside the grid.");
        if (AsteroidField[x][y] == States.Asteroid) 
            throw new BlockedLocationException($" Position ({x}, {y}) is blocked.");
    }

    /// <summary>
    /// Determines whether the provided location is Luna's destination.
    /// </summary>
    /// <param name="x">Vertical coordinate.</param>
    /// <param name="y">horizontal coordinate.</param>
    /// <returns>true/false</returns>
    public bool IsDestination(int x, int y)
    {
        if (!AsteroidField.Any())
            return true;
        if (AsteroidField[x][y] == States.End)
            return true;
        return false;
    }
}