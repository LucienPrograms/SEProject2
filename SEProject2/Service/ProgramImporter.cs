using SEProject2.Models;
using SEProject2.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SEProject2.Service
{
    public class ProgramImporter
    {
        public Program ImportFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            string[] lines = File.ReadAllLines(filePath);

            return new Program(

                Path.GetFileNameWithoutExtension(filePath),
                CommandFactory.Create(LineParser.ParseBlock(lines))
            );
        }
    }
}
