using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class BasicMazeGenerator : IMazeGenerator
    {
        private readonly IMazeDataAccess dataAccess;

        public BasicMazeGenerator(IMazeDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public char[,] GenerateMaze(string filePath)
        {
            string[] lines = dataAccess.ReadCSV(filePath);

            if (lines == null || lines.Length == 0)
            {
                return null; // Handle invalid CSV file or other errors
            }

            // Convert CSV data to maze representation
            char[,] maze = ConvertToMaze(lines);

            return maze;
        }

        private char[,] ConvertToMaze(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length; // Split the first line using commas to get the number of columns
            char[,] maze = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                // Split the line into individual values using commas as delimiters
                string[] values = lines[i].Split(',');

                for (int j = 0; j < cols; j++)
                {
                    // Convert the value to a character and handle invalid values
                    if (values.Length <= j || !char.TryParse(values[j], out char currentChar))
                    {
                        maze[i, j] = ' '; // Treat as an empty space for invalid or missing values
                    }
                    else if (currentChar == '1')
                    {
                        maze[i, j] = '#'; // '1' represents a wall
                    }
                    else
                    {
                        maze[i, j] = ' '; // Any other character represents an empty space
                    }
                }
            }

            return maze;
        }



    }
}
