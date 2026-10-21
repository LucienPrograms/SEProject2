using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SEProject2.Models
{
    public class Direction
    {
        public enum Heading
        {
            EAST,
            WEST,
            NORTH,
            SOUTH
        }
        public enum LeftRight
        {
            Left,
            Right
        }
        public static readonly Dictionary<Heading, Heading> Right = new Dictionary<Heading, Heading>
        {
            {Heading.NORTH, Heading.EAST},
            {Heading.EAST, Heading.SOUTH},
            {Heading.SOUTH, Heading.WEST},
            {Heading.WEST, Heading.NORTH}
        };
        public static readonly Dictionary<Heading, Heading> Left = new Dictionary<Heading, Heading>
        {
            {Heading.NORTH, Heading.WEST},
            {Heading.EAST, Heading.NORTH},
            {Heading.SOUTH, Heading.EAST},
            {Heading.WEST, Heading.SOUTH}
        };
        public static readonly Dictionary<Heading, (int, int)> Step = new Dictionary<Heading, (int, int)>
        {
            {Heading.NORTH, (0, 1)},
            {Heading.EAST, (1, 0)},
            {Heading.SOUTH, (0, -1)},
            {Heading.WEST, (-1, 0)}
        };
    }
}
