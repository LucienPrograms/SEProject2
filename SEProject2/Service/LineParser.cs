using System;
using System.Collections.Generic;
using System.Text;

namespace SEProject2.Service
{
    public class LineParser
    {
        public static List<ParsedLine> ParseBlock(string[] lines)
        {
            var parsed = new List<ParsedLine>();
            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                int indent = GetIndent(line);
                string trimmed = line.Trim();
                parsed.Add(new ParsedLine { Indent = indent, Raw = trimmed, Parts = trimmed.Split(' ') });
            }

            return parsed;
        }

        private static int GetIndent(string line)
        {
            int count = 0;
            foreach (char c in line)
            {
                if (c == ' ')
                    count++;
                else
                    break;
            }
            return count;
        }
    }
}
