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
    public class AddWallMazeGenerator : IAddWallMazeGenerator
    {
        private readonly Random random = new Random();

        public Maze GenerateMaze(int width, int height, int wallThickness)
        {
            // Create an empty maze
            Maze maze = new Maze(width, height, wallThickness);

            // Create nodes for each cell in the maze
            CreateNodes(maze);

            // Connect all adjacent cells (nodes) in the maze
            ConnectAllNodes(maze);

            // Add walls randomly
            //AddRandomWalls(maze);

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


        private void ConnectAllNodes(Maze maze)
        {
            int width = maze.Width;
            int height = maze.Height;

            // Connect all nodes to their adjacent neighbors (up, down, left, right)
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    MazeNode currentNode = maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col);

                    // Connect to the node above
                    if (row > 0)
                    {
                        MazeNode nodeAbove = maze.MazeGraph.Vertices.First(node => node.Row == row - 1 && node.Column == col);
                        maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeAbove));
                    }

                    // Connect to the node below
                    if (row < height - 1)
                    {
                        MazeNode nodeBelow = maze.MazeGraph.Vertices.First(node => node.Row == row + 1 && node.Column == col);
                        maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeBelow));
                    }

                    // Connect to the node to the left
                    if (col > 0)
                    {
                        MazeNode nodeLeft = maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col - 1);
                        maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeLeft));
                    }

                    // Connect to the node to the right
                    if (col < width - 1)
                    {
                        MazeNode nodeRight = maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col + 1);
                        maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeRight));
                    }
                }
            }
        }


        private void AddRandomWalls(Maze maze)
        {
            int maxWallCount = (maze.Width - 2 * maze.WallThickness) * (maze.Height - 2 * maze.WallThickness);
            int wallCount = random.Next(maxWallCount);

            for (int i = 0; i < wallCount; i++)
            {
                int row = random.Next(maze.WallThickness, maze.Height - maze.WallThickness);
                int col = random.Next(maze.WallThickness, maze.Width - maze.WallThickness);

                // Ensure the selected cell is not already a wall
                MazeNode node = new MazeNode(row, col, ' ');

                if (maze.MazeGraph.ContainsVertex(node))
                {
                    // Set the cell as a wall
                    maze.MazeGraph.RemoveVertex(node);
                    maze.MazeGraph.AddVertex(new MazeNode(row, col, '#'));
                }
            }
        }
    }
}
