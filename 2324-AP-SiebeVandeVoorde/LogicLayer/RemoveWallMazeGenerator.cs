using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Globals.Entities;
using Globals.Interfaces;
using QuickGraph;

namespace LogicLayer
{
    public class RemoveWallMazeGenerator : IMazeGenerator
    {
        private readonly Random random = new Random();

        public Maze GenerateMaze(int width, int height, int wallThickness)
        {
            // Create an empty maze
            Maze maze = new Maze(width, height, wallThickness);

            // Create nodes for each cell in the maze and initialize with walls
            CreateNodesWithWalls(maze);

            // Generate the maze using recursive backtracking
            GenerateMazeRecursive(maze, 1, 1);

            // Connect all adjacent cells (nodes) in the maze
            maze.ConnectAllNodes();

            return maze;
        }

        private void CreateNodesWithWalls(Maze maze)
        {
            for (int row = 0; row < maze.Height; row++)
            {
                for (int col = 0; col < maze.Width; col++)
                {
                    maze.MazeGraph.AddVertex(new MazeNode(row, col, '1'));
                }
            }
        }

        private void GenerateMazeRecursive(Maze maze, int currentRow, int currentCol)
        {
            // Mark the current cell as open
            maze.MazeGraph.Vertices.First(node => node.Row == currentRow && node.Column == currentCol).Value = '0';

            // Get a random order for the directions (up, down, left, right)
            var directions = new List<(int, int)> { (-1, 0), (1, 0), (0, -1), (0, 1) };
            Shuffle(directions);

            foreach (var (dx, dy) in directions)
            {
                int nextRow = currentRow + 2 * dy;
                int nextCol = currentCol + 2 * dx;

                if (IsValidCell(maze, nextRow, nextCol))
                {
                    // Mark the cell between the current and next cell as open
                    maze.MazeGraph.Vertices.First(node => node.Row == currentRow + dy && node.Column == currentCol + dx).Value = '0';

                    // Recursively visit the next cell
                    GenerateMazeRecursive(maze, nextRow, nextCol);
                }
            }
        }

        private bool IsValidCell(Maze maze, int row, int col)
        {
            return row >= 1 && row < maze.Height - 1 && col >= 1 && col < maze.Width - 1 && maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col).Value == '1';
        }

        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + random.Next(n - i);
                T temp = list[i];
                list[i] = list[r];
                list[r] = temp;
            }
        }
    }
}
