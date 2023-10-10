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
            Maze maze = new Maze(width, height, wallThickness);
            CreateNodesWithWalls(maze);
            GenerateMazeRecursive(maze, 0, 0);
            maze.ConnectAllNodes();

            return maze;
        }

        private void CreateNodesWithWalls(Maze maze)
        {
            for (int row = 0; row < maze.Height; row++)
            {
                for (int col = 0; col < maze.Width; col++)
                {
                    maze.MazeGraph.AddVertex(new MazeNode(row, col, "1111"));
                }
            }
        }

        private void GenerateMazeRecursive(Maze maze, int currentRow, int currentCol)
        {
            MazeNode currentNode = maze.MazeGraph.Vertices.First(node => node.Row == currentRow && node.Column == currentCol);

            var directions = new List<(int, int)> { (-1, 0), (1, 0), (0, -1), (0, 1) };
            Shuffle(directions);

            foreach (var (dx, dy) in directions)
            {
                int nextRow = currentRow + 1 * dy;
                int nextCol = currentCol + 1 * dx;

                if (IsValidCell(maze, nextRow, nextCol))
                {
                    MazeNode nextNode = maze.MazeGraph.Vertices.First(node => node.Row == currentRow + dy && node.Column == currentCol + dx);

                    // node to the right
                    if (dx == 1)
                    {
                        currentNode.Value = currentNode.Value.Remove(1, 1).Insert(1, "0");
                        nextNode.Value = nextNode.Value.Remove(3, 1).Insert(3, "0");
                    }
                    // node to the left
                    else if (dx == -1)
                    {
                        currentNode.Value = currentNode.Value.Remove(3, 1).Insert(3, "0");
                        nextNode.Value = nextNode.Value.Remove(1, 1).Insert(1, "0");
                    }
                    // node below
                    else if (dy == 1)
                    {
                        currentNode.Value = currentNode.Value.Remove(2, 1).Insert(2, "0");
                        nextNode.Value = nextNode.Value.Remove(0, 1).Insert(0, "0");
                    }
                    // node above
                    else if (dy == -1)
                    {
                        currentNode.Value = currentNode.Value.Remove(0, 1).Insert(0, "0");
                        nextNode.Value = nextNode.Value.Remove(2, 1).Insert(2, "0");
                    }

                    GenerateMazeRecursive(maze, nextRow, nextCol);
                }
            }
        }

        private bool IsValidCell(Maze maze, int row, int col)
        {
            return row >= 0 && row < maze.Height && col >= 0 && col < maze.Width &&
                maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col).Value == "1111";
            /*if (row >= 0 && row < maze.Height && col >= 0 && col < maze.Width)
            {
                MazeNode node = maze.MazeGraph.Vertices.First(node => node.Row == row && node.Column == col);
                if (node.Value == "1111")
                {
                    return true;
                }
                else { return false; }
            }
            else { return false; }*/
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
