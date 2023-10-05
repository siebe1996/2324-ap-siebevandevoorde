using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Entities
{
    public class Maze
    {
        public int Width { get; }
        public int Height { get; }
        public int WallThickness { get; }
        public Coordinate BallPosition { get; set; }
        public UndirectedGraph<MazeNode, Edge<MazeNode>> MazeGraph { get; }

        public Maze(int width, int height, int wallThickness)
        {
            if (width <= 0 || height <= 0 || wallThickness < 0)
            {
                throw new ArgumentException("Invalid maze dimensions or wall thickness.");
            }

            Width = width;
            Height = height;
            WallThickness = wallThickness;
            BallPosition = new Coordinate(1, 1); // Initialize ball position to (1, 1)
            MazeGraph = new UndirectedGraph<MazeNode, Edge<MazeNode>>();
        }

        public void MoveBall(int deltaX, int deltaY)
        {
            // Update the ball's position within the maze
            int newX = BallPosition.X + deltaX;
            int newY = BallPosition.Y + deltaY;

            // Ensure the new position is within the maze boundaries
            if (newX >= 0 && newX < Width && newY >= 0 && newY < Height)
            {
                BallPosition = new Coordinate(newX, newY);
            }
        }

        public void ConnectAllNodes()
        {
            int width = this.Width;
            int height = this.Height;

            // Connect all nodes to their adjacent neighbors (up, down, left, right)
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    MazeNode currentNode = this.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col);

                    // Check if the current node is an open cell ('0')
                    if (currentNode.Value == '0')
                    {
                        // Connect to the node above if it's an open cell ('0')
                        if (row > 0)
                        {
                            MazeNode nodeAbove = this.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row - 1 && node.Column == col && node.Value == '0');
                            if (nodeAbove != null)
                            {
                                this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeAbove));
                            }
                        }

                        // Connect to the node below if it's an open cell ('0')
                        if (row < height - 1)
                        {
                            MazeNode nodeBelow = this.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row + 1 && node.Column == col && node.Value == '0');
                            if (nodeBelow != null)
                            {
                                this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeBelow));
                            }
                        }

                        // Connect to the node to the left if it's an open cell ('0')
                        if (col > 0)
                        {
                            MazeNode nodeLeft = this.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row && node.Column == col - 1 && node.Value == '0');
                            if (nodeLeft != null)
                            {
                                this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeLeft));
                            }
                        }

                        // Connect to the node to the right if it's an open cell ('0')
                        if (col < width - 1)
                        {
                            MazeNode nodeRight = this.MazeGraph.Vertices.FirstOrDefault(node => node.Row == row && node.Column == col + 1 && node.Value == '0');
                            if (nodeRight != null)
                            {
                                this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeRight));
                            }
                        }
                    }
                }
            }
        }
    }
}
