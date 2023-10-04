using Globals.Entities;
using Globals.Interfaces;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LogicLayer
{
    public class AddWallMazeGenerator : IMazeGenerator
    {
        private readonly Random random = new Random();

        public Maze GenerateMaze(int width, int height, int wallThickness)
        {
            // Create an empty maze
            Maze maze = new Maze(width, height, wallThickness);

            // Create nodes for each cell in the maze
            CreateNodes(maze);

            AddRandomWalls(maze);

            maze.ConnectAllNodes();

            return maze;
        }

        private void CreateNodes(Maze maze)
        {
            for (int row = 0; row < maze.Height; row++)
            {
                for (int col = 0; col < maze.Width; col++)
                {
                    maze.MazeGraph.AddVertex(new MazeNode(row, col, '0'));
                }
            }
        }


        private void AddRandomWalls(Maze maze)
        {
            int width = maze.Width;
            int height = maze.Height;

            // Add walls along the top and bottom edges
            for (int col = 0; col < width; col++)
            {
                maze.MazeGraph.Vertices.First(node => node.Row == 0 && node.Column == col).Value = '1'; // Top edge
                maze.MazeGraph.Vertices.First(node => node.Row == height - 1 && node.Column == col).Value = '1'; // Bottom edge
            }

            // Add walls along the left and right edges (excluding corners)
            for (int row = 1; row < height - 1; row++)
            {
                maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == 0).Value = '1'; // Left edge
                maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == width - 1).Value = '1'; // Right edge
            }

            int numWallsToAdd = (int)((width - 2) * (height - 2) * 0.3);

            for (int i = 0; i < numWallsToAdd; i++)
            {
                int randomRow = random.Next(1, height - 1);
                int randomCol = random.Next(1, width - 1);

                // Find the MazeNode at the random position and set its value to '1' to represent a wall
                MazeNode randomNode = maze.MazeGraph.Vertices.First(node => node.Row == randomRow && node.Column == randomCol);
                randomNode.Value = '1';
            }
        }


    }
}
