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

            AddRandomWalls(maze);

            // Connect all adjacent cells (nodes) in the maze
            ConnectAllNodes(maze);

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

                    // Check if the current node is an open cell ('0')
                    if (currentNode.Value == '0')
                    {
                        // Connect to the node above if it's an open cell ('0')
                        if (row > 0)
                        {
                            MazeNode nodeAbove = maze.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row - 1 && node.Column == col && node.Value == '0');
                            if (nodeAbove != null)
                            {
                                maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeAbove));
                            }
                        }

                        // Connect to the node below if it's an open cell ('0')
                        if (row < height - 1)
                        {
                            MazeNode nodeBelow = maze.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row + 1 && node.Column == col && node.Value == '0');
                            if (nodeBelow != null)
                            {
                                maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeBelow));
                            }
                        }

                        // Connect to the node to the left if it's an open cell ('0')
                        if (col > 0)
                        {
                            MazeNode nodeLeft = maze.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row && node.Column == col - 1 && node.Value == '0');
                            if (nodeLeft != null)
                            {
                                maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeLeft));
                            }
                        }

                        // Connect to the node to the right if it's an open cell ('0')
                        if (col < width - 1)
                        {
                            MazeNode nodeRight = maze.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row && node.Column == col + 1 && node.Value == '0');
                            if (nodeRight != null)
                            {
                                maze.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeRight));
                            }
                        }
                    }
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

        private int CalculateNumWallsToAdd(int width, int height, int wallThickness)
        {
            // Calculate the total number of cells and the percentage of walls based on wall thickness
            int totalCells = width * height;
            double wallPercentage = wallThickness / 100.0;

            // Calculate the number of walls to add as a percentage of total cells
            int numWallsToAdd = (int)(totalCells * wallPercentage);

            // Ensure numWallsToAdd is within a reasonable range
            numWallsToAdd = Math.Min(Math.Max(numWallsToAdd, 0), totalCells - 1);

            return numWallsToAdd;
        }
    







        private List<MazeNode> GetAdjacentNodes(Maze maze, MazeNode node)
        {
            int row = node.Row;
            int col = node.Column;
            List<MazeNode> adjacentNodes = new List<MazeNode>();

            if (row > 0)
            {
                adjacentNodes.Add(maze.MazeGraph.Vertices.First(n => n.Row == row - 1 && n.Column == col));
            }
            if (row < maze.Height - 1)
            {
                adjacentNodes.Add(maze.MazeGraph.Vertices.First(n => n.Row == row + 1 && n.Column == col));
            }
            if (col > 0)
            {
                adjacentNodes.Add(maze.MazeGraph.Vertices.First(n => n.Row == row && n.Column == col - 1));
            }
            if (col < maze.Width - 1)
            {
                adjacentNodes.Add(maze.MazeGraph.Vertices.First(n => n.Row == row && n.Column == col + 1));
            }

            return adjacentNodes;
        }


    }
}
