using System;
using System.Collections.Generic;
using System.Text;

namespace SEProject2.Models
{
    public class Character
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public Direction Facing { get; private set; }

        private readonly List <String> executedCommandTrace = new List<string>();

        public Character()
        {
            ResetToStart();
        }

        public void ResetToStart()
        {
            X = 0;
            Y = 0;
            Facing = Direction.EAST;
            executedCommandTrace.Clear();
        }

        public void MoveForward(int steps)
        {
            if (steps < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steps),"Steps cannot be negative");
            }

            for (int i = 0; i < steps; i++)
            {

                switch (Facing)
                {
                    case Direction.NORTH:
                        Y += 1;
                        break;
                    case Direction.EAST:
                        X += 1;
                        break;
                    case Direction.SOUTH:
                        Y -= 1;
                        break;
                    case Direction.WEST:
                        X -= 1;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown direction");
                }
            }

            executedCommandTrace.Add($"Move {steps}");
        }

        public void TurnLeft()
        {
            switch (Facing)
            {
                case Direction.NORTH:
                    Facing = Direction.WEST;
                    break;
                case Direction.EAST:
                    Facing = Direction.NORTH;
                    break;
                case Direction.SOUTH:
                    Facing = Direction.EAST;
                    break;
                case Direction.WEST:
                    Facing = Direction.SOUTH;
                    break;
                default:
                    throw new InvalidOperationException("Unknown direction");
            }

            executedCommandTrace.Add("Turn left");
        }

        public void TurnRight()
        {
            switch (Facing)
            {
                case Direction.NORTH:
                    Facing = Direction.EAST;
                    break;
                case Direction.EAST:
                    Facing = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    Facing = Direction.WEST;
                    break;
                case Direction.WEST:
                    Facing = Direction.NORTH;
                    break;
                default:
                    throw new InvalidOperationException("Unknown direction");
            }

            executedCommandTrace.Add("Turn right");
        }

        public string GetState()
        {
            return $"({X}, {Y}) facing {Facing.ToString().ToLower()}";
        }

        public IReadOnlyList<string> GetTraceEntries()
        {
            return executedCommandTrace.AsReadOnly();
        }
    }
}
