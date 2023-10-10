using Globals.Interfaces;
using Globals.Entities;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class BasicMazeGenerator : IBasicMazeGenerator
    {
        private readonly IMazeDataAccess dataAccess;

        public BasicMazeGenerator(IMazeDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        /*public char[,] GenerateMaze(string filePath)
        {
            string[] lines = dataAccess.ReadCSV(filePath); 

            if (lines == null || lines.Length == 0)
            {
                return null;
            }

            char[,] maze = ConvertToMaze(lines);

            return maze;
        }*/

        public Maze GenerateGraphMaze(string filePath, int wallThickness)
        {
            string[] lines = dataAccess.ReadCSV(filePath);

            if (lines == null || lines.Length == 0)
            {
                return null;
            }

            Maze maze = ConvertToGraphMaze(lines, wallThickness);

            return maze;
        }

        /*private char[,] ConvertToMaze(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;
            char[,] maze = new char[rows, cols];

            if (!(lines.Length > 1 && lines[1].Split(',')[1][0] == '0'))
            {
                throw new InvalidOperationException("Position (1, 1) is not '0'");
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    string[] values = lines[i].Split(',');

                    for (int j = 0; j < cols; j++)
                    {
                        if (values.Length <= j || !char.TryParse(values[j], out char currentChar))
                        {
                            maze[i, j] = ' ';
                        }
                        else if (currentChar == '1')
                        {
                            maze[i, j] = '#';
                        }
                        else
                        {
                            maze[i, j] = ' ';
                        }
                    }
                }

                return maze;
            }
            }*/

            private Maze ConvertToGraphMaze(string[] lines, int wallThickness)
        {

            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;

            /*if (!(lines.Length > 1 && lines[1].Split(',')[1][0] == '0'))
            {
                throw new InvalidOperationException("Position (1, 1) is not '0'");
            }*/
            Maze maze = new Maze(cols, rows, wallThickness);


            MazeNode[,] nodes = new MazeNode[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                string[] values = lines[i].Split(',');

                for (int j = 0; j < cols; j++)
                {
                    string currentValue = values[j];

                    MazeNode node = new MazeNode(i, j, currentValue);
                    nodes[i, j] = node;

                    maze.MazeGraph.AddVertex(node);

                }
            }

            maze.ConnectAllNodes();

            return maze;
            /*else
            {

                Maze maze = new Maze(cols, rows, wallThickness);


                MazeNode[,] nodes = new MazeNode[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    string[] values = lines[i].Split(',');

                    for (int j = 0; j < cols; j++)
                    {
                        char currentChar = values[j][0];

                        MazeNode node = new MazeNode(i, j, currentChar);
                        nodes[i, j] = node;

                        maze.MazeGraph.AddVertex(node);

                    }
                }

                maze.ConnectAllNodes();

                return maze;
            }*/
        }



    }

}

