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
            Maze maze = new Maze(width, height, wallThickness);

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
                    maze.MazeGraph.AddVertex(new MazeNode(row, col, "0000"));
                }
            }
        }

        private void AddRandomWalls(Maze maze)
        {
            int width = maze.Width;
            int height = maze.Height;
            Random random = new Random();

            int totalCells = (width) * (height);


            for (int i = 0; i < totalCells; i++)
            {
                int randomRow = random.Next(1, height - 1);
                int randomCol = random.Next(1, width - 1);

                string binaryValue = Convert.ToString(random.Next(16), 2).PadLeft(4, '0');
                
                MazeNode currentNode = maze.MazeGraph.Vertices.First(node => node.Row == randomRow && node.Column == randomCol);
                currentNode.Value = binaryValue;
                maze.ChangeOtherNode(currentNode);
            }

            // i should set the outside of the border nodes to one but this isnt necessary because they cant connect to any nodes so they dont have edges
        }

    }
}
