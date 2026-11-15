namespace SEProject2.Models;

class Space
{
    int Height { get; }
    int Width { get; }
    States[][] AsteroidField { get; }

    static enum States { Blocked, Open, End }

    public Grid(string input)
    {
        string[] lines = input.Split("\n");

        if (!CheckLineLength(lines)) throw new Exception("Line lengths do not match.");

        Height = lines.Length; Width = lines[0].Length;
        AsteroidField = new int[Height][Width];
        ParseGrid(lines);
    }

    public bool Collision (int x, int y)
    {
        if (x < 0 || x > Height || y < 0 || y > Width) throw new Exception("Out of Bounds Exception");
        if (AsteroidField[x][y] == States.Blocked) throw new Exception("Blocked ");
        return false;
    }

    private States[] ParseGrid(string[] lines)
    {
        int row = 0;
        foreach (string line in lines)
        {
            AsteroidField[row] = ParseLine(line);   
            row++;
        }
    }

    private States[] ParseLine(string line)
    {
        int column = 0;
        States[] ParsedLine = new States[line.Length];
        foreach (char character in line)
        {
            ParsedLine[column] = ConvertToState(character);
            column++;
        }
        return ParsedLine;
    }

    private States? ConvertToState(char character) => character switch
    {
        '+' => States.Blocked,
        'o' => States.Open,
        'x' => States.End,
        _ => throw new Exception("Invalid grid character")
    };

    private bool CheckLineLength(string[] lines)
    {
        int length = lines[0].Length;
        foreach (string line in lines.Skip(1))
        {
            if (line.Length != length) return false;
        }
        return true;
    }

}