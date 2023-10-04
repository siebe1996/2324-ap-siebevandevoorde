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

        public char[,] GenerateMaze(string filePath)
        {
            string[] lines = dataAccess.ReadCSV(filePath); 

            if (lines == null || lines.Length == 0)
            {
                return null; // Handle invalid CSV file or other errors
            }

            // Convert CSV data to maze representation
            char[,] maze = ConvertToMaze(lines);

            return maze;
        }

        public Maze GenerateGraphMaze(string filePath)
        {
            string[] lines = dataAccess.ReadCSV(filePath);

            if (lines == null || lines.Length == 0)
            {
                return null; // Handle invalid CSV file or other errors
            }

            // Convert CSV data to a maze represented as an undirected graph
            Maze maze = ConvertToGraphMaze(lines);

            return maze;
        }

        private char[,] ConvertToMaze(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length; // Split the first line using commas to get the number of columns
            char[,] maze = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                // Split the line into individual values using commas as delimiters
                string[] values = lines[i].Split(',');

                for (int j = 0; j < cols; j++)
                {
                    // Convert the value to a character and handle invalid values
                    if (values.Length <= j || !char.TryParse(values[j], out char currentChar))
                    {
                        maze[i, j] = ' '; // Treat as an empty space for invalid or missing values
                    }
                    else if (currentChar == '1')
                    {
                        maze[i, j] = '#'; // '1' represents a wall
                    }
                    else
                    {
                        maze[i, j] = ' '; // Any other character represents an empty space
                    }
                }
            }

            return maze;
        }

        private Maze ConvertToGraphMaze(string[] lines)
        {
            //var mazeGraph = new UndirectedGraph<MazeNode, Edge<MazeNode>>();

            int rows = lines.Length;
            int cols = lines[0].Split(',').Length; // Split the first line using commas to get the number of columns

            Maze maze = new Maze(cols, rows, 4); // Create a new Maze object with the specified dimensions and wall thickness


            // Create nodes for each cell in the maze
            MazeNode[,] nodes = new MazeNode[rows, cols];
            
            for (int i = 0; i < rows; i++)
            {
                string[] values = lines[i].Split(',');

                for (int j = 0; j < cols; j++)
                {
                    char currentChar = values[j][0];

                    // Create a node for each cell
                    MazeNode node = new MazeNode(i, j, currentChar);
                    nodes[i, j] = node;

                    // Add the node to the graph
                    maze.MazeGraph.AddVertex(node);

                }
            }

            for (int i = 0; i < rows; i++)
            {

                for (int j = 0; j < cols; j++)
                {
                    char currentChar = nodes[i,j].Value;

                    // Connect nodes to adjacent open cells (no walls)
                    if (currentChar == '0')
                    {
                        // Check and connect to the left
                        if (j > 0 && nodes[i, j - 1].Value == '0')
                        {
                            maze.MazeGraph.AddEdge(new Edge<MazeNode>(nodes[i, j], nodes[i, j - 1]));
                        }

                        // Check and connect upward
                        if (i > 0 && nodes[i - 1, j].Value == '0')
                        {
                            maze.MazeGraph.AddEdge(new Edge<MazeNode>(nodes[i, j], nodes[i - 1, j]));
                        }

                        // Check and connect to right
                        if (j < cols - 1 && nodes[i, j + 1].Value == '0')
                        {
                            maze.MazeGraph.AddEdge(new Edge<MazeNode>(nodes[i, j], nodes[i, j + 1]));
                        }

                        // Check and connect downward
                        if (i < rows -1 && nodes[i + 1, j].Value == '0')
                        {
                            maze.MazeGraph.AddEdge(new Edge<MazeNode>(nodes[i, j], nodes[i + 1, j]));
                        }
                    }
                }
            }

            return maze;
        }



    }

}

