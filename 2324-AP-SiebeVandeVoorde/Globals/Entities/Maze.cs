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
            BallPosition = new Coordinate(0, 0); // Initialize ball position to (0, 0)
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
    }
}
