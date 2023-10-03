using Globals.Entities;
using Globals.Interfaces;
using LogicLayer;
using Microsoft.Win32;
using QuickGraph;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        private readonly IMazeGenerator mazeGenerator;

        public MainWindow(IMazeGenerator mazeGenerator)
        {
            InitializeComponent();
            this.mazeGenerator = mazeGenerator;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void GenerateMazeButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            char[,] maze = mazeGenerator.GenerateMaze(filePath);

            // Clear previous maze (if any)
            MazeCanvas.Children.Clear();

            if (maze != null)
            {
                // Display the maze on the canvas
                // You can implement a method to draw the maze grid here
                DrawMaze(maze);
            }
            else
            {
                MessageBox.Show("Invalid CSV file or other error occurred.");
            }
        }

        private void GenerateGraphMazeButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            UndirectedGraph<MazeNode, Edge<MazeNode>> maze = mazeGenerator.GenerateGraphMaze(filePath);

            // Clear previous maze (if any)
            MazeCanvas.Children.Clear();

            if (maze != null)
            {
                // Display the maze on the canvas
                // You can implement a method to draw the maze grid here
                DrawGraphMaze(maze);
            }
            else
            {
                MessageBox.Show("Invalid CSV file or other error occurred.");
            }
        }

        private void DrawMaze(char[,] maze)
        {
            // Determine the dimensions of the maze
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);


            // Calculate the cell size based on the smaller dimension to ensure it fits entirely
            double cellSize = Math.Min(MazeCanvas.ActualWidth / cols, MazeCanvas.ActualHeight / rows);


            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char cell = maze[i, j];
                    if (cell == '#')
                    {
                        // Create a rectangle for walls
                        Rectangle wallRect = new Rectangle
                        {
                            Width = cellSize,
                            Height = cellSize,
                            Fill = Brushes.Black // Color for walls
                        };

                        // Position the rectangle at the correct cell location
                        double x = j * cellSize;
                        double y = i * cellSize;
                        Canvas.SetLeft(wallRect, x);
                        Canvas.SetTop(wallRect, y);

                        // Add the rectangle to the canvas
                        MazeCanvas.Children.Add(wallRect);
                    }
                    else if (cell == ' ')
                    {
                        // Create a rectangle for open spaces (the road)
                        Rectangle openRect = new Rectangle
                        {
                            Width = cellSize,
                            Height = cellSize,
                            Fill = Brushes.Orange // Color for open spaces (orange)
                        };

                        // Position the rectangle at the correct cell location
                        double x = j * cellSize;
                        double y = i * cellSize;
                        Canvas.SetLeft(openRect, x);
                        Canvas.SetTop(openRect, y);

                        // Add the rectangle to the canvas
                        MazeCanvas.Children.Add(openRect);
                    }
                }
            }
        }


        public void DrawGraphMaze(UndirectedGraph<MazeNode, Edge<MazeNode>> mazeGraph)
        {
            if (mazeGraph == null)
                throw new ArgumentNullException(nameof(mazeGraph));

            // Clear the canvas before drawing the maze
            MazeCanvas.Children.Clear();

            // Makes the cells bigger when lower
            double cellSizeMultiplier = 0.25;

            // Determine the cell size based on the smaller dimension to ensure it fits entirely
            double cellSize = Math.Min(MazeCanvas.ActualWidth / (mazeGraph.VertexCount * cellSizeMultiplier), MazeCanvas.ActualHeight / (mazeGraph.VertexCount * cellSizeMultiplier));


            foreach (var vertex in mazeGraph.Vertices)
            {
                // Create a rectangle to represent the maze node
                var nodeRectangle = new Rectangle
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = vertex.Value == '1' ? Brushes.Red : Brushes.Orange,
                    Stroke = Brushes.Blue,
                    StrokeThickness = 2
                };

                // Set the position of the rectangle on the canvas
                Canvas.SetLeft(nodeRectangle, vertex.Column * cellSize);
                Canvas.SetTop(nodeRectangle, vertex.Row * cellSize);

                // Add the rectangle to the canvas
                MazeCanvas.Children.Add(nodeRectangle);
            }

            foreach (var edge in mazeGraph.Edges)
            {
                // Calculate the coordinates of the edge's start and end points
                double startX = edge.Source.Column * cellSize + cellSize / 2;
                double startY = edge.Source.Row * cellSize + cellSize / 2;
                double endX = edge.Target.Column * cellSize + cellSize / 2;
                double endY = edge.Target.Row * cellSize + cellSize / 2;

                // Create a line to represent the edge
                var edgeLine = new Line
                {
                    X1 = startX,
                    Y1 = startY,
                    X2 = endX,
                    Y2 = endY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                // Add the line to the canvas
                MazeCanvas.Children.Add(edgeLine);
            }
        }
    }




}

