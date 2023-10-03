using Globals.Interfaces;
using LogicLayer;
using Microsoft.Win32;
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







    }
}

