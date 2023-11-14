using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public Ball Ball { get; set; }

        public Maze(int width, int height, int wallThickness)
        {
            if (width <= 0 || height <= 0 || wallThickness < 0)
            {
                throw new ArgumentException("Invalid maze dimensions or wall thickness.");
            }

            Width = width;
            Height = height;
            WallThickness = wallThickness;
            BallPosition = new Coordinate(0, 0);
            MazeGraph = new UndirectedGraph<MazeNode, Edge<MazeNode>>();
            Ball = new Ball(0);
        }

        public void MoveBall(double deltaX, double deltaY, double deltaZ)
        {
            double newX = BallPosition.X + deltaX;
            double newY = BallPosition.Y + deltaY;
            double? newZ = BallPosition.Z.HasValue? BallPosition.Z.Value + deltaZ :  null;

            if (newX >= 0 && newX < Width && newY >= 0 && newY < Height && newZ >= 0+Ball.Straal)
            {
                BallPosition = new Coordinate(newX, newY, newZ);
            }
        }

        public void ConnectAllNodes()
        {
            int width = this.Width;
            int height = this.Height;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    MazeNode currentNode = this.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col);

                    // Connect to the node above
                    MazeNode nodeAbove = GetNeighbor(currentNode, -1, 0);
                    if (nodeAbove != null && new Regex(@"^0").IsMatch(currentNode.Value))
                    {
                        this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeAbove));
                    }

                    // Connect to the node to the right
                    MazeNode nodeRight = GetNeighbor(currentNode, 0, 1);
                    if (nodeRight != null && new Regex(@"^.0").IsMatch(currentNode.Value))
                    {
                        this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeRight));
                    }

                    // Connect to the node below
                    MazeNode nodeBelow = GetNeighbor(currentNode, 1, 0);
                    if (nodeBelow != null && new Regex(@"^.{2}0").IsMatch(currentNode.Value))
                    {
                        this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeBelow));
                    }

                    // Connect to the node to the left
                    MazeNode nodeLeft = GetNeighbor(currentNode, 0, -1);
                    if (nodeLeft != null && new Regex(@"^.{3}0").IsMatch(currentNode.Value))
                    {
                        this.MazeGraph.AddEdge(new Edge<MazeNode>(currentNode, nodeLeft));
                    }
                }
            }
        }


        public void ChangeOtherNode(MazeNode currentNode)
        {
            int width = this.Width;
            int height = this.Height;

            // Change the node above
            MazeNode nodeAbove = GetNeighbor(currentNode, -1, 0);
            if (nodeAbove != null && new Regex(@"^1").IsMatch(currentNode.Value))
            {
                char[] charArray = nodeAbove.Value.ToCharArray();
                charArray[0] = '1';
                string newValue = new string(charArray);
                nodeAbove.Value = newValue;
            }

            // Change the right
            MazeNode nodeRight = GetNeighbor(currentNode, 0, 1);
            if (nodeRight != null && new Regex(@"^.0").IsMatch(currentNode.Value))
            {
                char[] charArray = nodeRight.Value.ToCharArray();
                charArray[1] = '1';
                string newValue = new string(charArray);
                nodeRight.Value = newValue;
            }

            // Change the node below
            MazeNode nodeBelow = GetNeighbor(currentNode, 1, 0);
            if (nodeBelow != null && new Regex(@"^.{2}0").IsMatch(currentNode.Value))
            {
                char[] charArray = nodeBelow.Value.ToCharArray();
                charArray[2] = '1';
                string newValue = new string(charArray);
                nodeBelow.Value = newValue;
            }

            // Change the node left
            MazeNode nodeLeft = GetNeighbor(currentNode, 0, -1);
            if (nodeLeft != null && new Regex(@"^.{3}0").IsMatch(currentNode.Value))
            {
                char[] charArray = nodeLeft.Value.ToCharArray();
                charArray[3] = '1';
                string newValue = new string(charArray);
                nodeLeft.Value = newValue;
            }
        }


        public MazeNode GetNeighbor(MazeNode node, int rowOffset, int colOffset)
        {
            int newRow = node.Row + rowOffset;
            int newCol = node.Column + colOffset;
            if (newRow < 0 || newRow >= this.Height || newCol < 0 || newCol >= this.Width)
            {
                return null;
            }

            return this.MazeGraph.Vertices.FirstOrDefault(v => v.Row == newRow && v.Column == newCol);
        }
    }
}
