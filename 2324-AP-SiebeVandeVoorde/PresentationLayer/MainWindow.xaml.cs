using Globals.Entities;
using Globals.Interfaces;
using LogicLayer;
using Microsoft.Win32;
using QuickGraph;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        private readonly IBasicMazeGenerator basicMazeGenerator;
        private readonly IMazeGenerator addWallMazeGenerator;
        private readonly IMazeGenerator removeWallMazeGenerator;

        public MainWindow(IBasicMazeGenerator basicMazeGenerator, IMazeGenerator addWallMazeGenerator, IMazeGenerator removeWallMazeGenerator)
        {
            InitializeComponent();
            this.basicMazeGenerator = basicMazeGenerator;
            this.addWallMazeGenerator = addWallMazeGenerator;
            this.removeWallMazeGenerator = removeWallMazeGenerator;
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

        private void GenerateGraphMazeButton_Click(object sender, RoutedEventArgs e)
        {
            string wallThinkness = WallThicknessTextBox1.Text;

            if (int.TryParse(wallThinkness, out int intValue))
            {
                string filePath = FilePathTextBox.Text;
                try
                {
                    Maze maze = basicMazeGenerator.GenerateGraphMaze(filePath, int.Parse(wallThinkness));

                    MazeCanvas.Children.Clear();

                    if (maze != null)
                    {
                        DrawGraphMaze(maze);
                    }
                    else
                    {
                        MessageBox.Show("Invalid CSV file or other error occurred.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Give number for wall thickness");
            }
        }

        private void GenerateAddWallGraphMazeButton_Click(object sender, RoutedEventArgs e)
        {
            string wallThinkness = WallThicknessTextBox2.Text;

            if (int.TryParse(wallThinkness, out int intValue))
            {
                Maze maze = addWallMazeGenerator.GenerateMaze(5, 5, int.Parse(wallThinkness));

                MazeCanvas.Children.Clear();

                if (maze != null)
                {
                    DrawGraphMaze(maze);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            else
            {
                MessageBox.Show("Give number for wall thickness");
            }
        }

        private void GenerateRemoveWallGraphMazeButton_Click(object sender, RoutedEventArgs e)
        {
            string wallThinkness = WallThicknessTextBox3.Text;

            if (int.TryParse(wallThinkness, out int intValue))
            {
                Maze maze = removeWallMazeGenerator.GenerateMaze(5, 5, int.Parse(wallThinkness));

                MazeCanvas.Children.Clear();

                if (maze != null)
                {
                    DrawGraphMaze(maze);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            else
            {
                MessageBox.Show("Give number for wall thickness");
            }
        }
        
        public void DrawGraphMaze(Maze maze)
        {
            if (maze == null)
                throw new ArgumentNullException(nameof(maze));

            MazeCanvas.Children.Clear();

            double cellSize = Math.Min((MazeCanvas.ActualWidth) / maze.MazeGraph.VertexCount, (MazeCanvas.ActualHeight) / maze.MazeGraph.VertexCount);
            cellSize = cellSize * 2;

            foreach (var vertex in maze.MazeGraph.Vertices)
            {
                double topPosition = vertex.Row * cellSize;
                double rightPosition = (vertex.Column + 1) * cellSize - maze.WallThickness;
                double bottomPosition = (vertex.Row + 1) * cellSize - maze.WallThickness;
                double leftPosition = vertex.Column * cellSize;

                bool isConnectedAbove = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, -1, 0));
                bool isConnectedRight = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, 1));
                bool isConnectedBelow = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 1, 0));
                bool isConnectedLeft = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, -1));

                var cellRectangle = new Rectangle
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = Brushes.Black,
                };
                Canvas.SetTop(cellRectangle, topPosition);
                Canvas.SetLeft(cellRectangle, leftPosition);
                MazeCanvas.Children.Add(cellRectangle);

                if (!isConnectedAbove)
                {
                    var aboveBorder = new Border
                    {
                        Width = cellSize,
                        Height = maze.WallThickness,
                        Background = Brushes.Red,
                    };
                    Canvas.SetTop(aboveBorder, topPosition);
                    Canvas.SetLeft(aboveBorder, leftPosition);
                    MazeCanvas.Children.Add(aboveBorder);
                }

                if (!isConnectedRight)
                {
                    var rightBorder = new Border
                    {
                        Width = maze.WallThickness,
                        Height = cellSize,
                        Background = Brushes.Red,
                    };
                    Canvas.SetTop(rightBorder, topPosition);
                    Canvas.SetLeft(rightBorder, rightPosition);
                    MazeCanvas.Children.Add(rightBorder);
                }

                if (!isConnectedBelow)
                {
                    var belowBorder = new Border
                    {
                        Width = cellSize,
                        Height = maze.WallThickness,
                        Background = Brushes.Red,
                    };
                    Canvas.SetTop(belowBorder, bottomPosition);
                    Canvas.SetLeft(belowBorder, leftPosition);
                    MazeCanvas.Children.Add(belowBorder);
                }

                if (!isConnectedLeft)
                {
                    var leftBorder = new Border
                    {
                        Width = maze.WallThickness,
                        Height = cellSize,
                        Background = Brushes.Red,
                    };
                    Canvas.SetTop(leftBorder, topPosition);
                    Canvas.SetLeft(leftBorder, leftPosition);
                    MazeCanvas.Children.Add(leftBorder);
                }
                if (vertex.Row == maze.BallPosition.X && vertex.Column == maze.BallPosition.Y)
                {
                    var ballEllipse = new Ellipse
                    {
                        Width = cellSize / 2,
                        Height = cellSize / 2,
                        Fill = Brushes.Blue,
                    };
                    Canvas.SetTop(ballEllipse, topPosition + cellSize / 4);
                    Canvas.SetLeft(ballEllipse, leftPosition + cellSize / 4);
                    MazeCanvas.Children.Add(ballEllipse);
                }
            }
            foreach (var edge in maze.MazeGraph.Edges)
            {
                var edgeLine = new Line
                {
                    X1 = edge.Source.Column * cellSize + cellSize / 2,
                    Y1 = edge.Source.Row * cellSize + cellSize / 2,
                    X2 = edge.Target.Column * cellSize + cellSize / 2,
                    Y2 = edge.Target.Row * cellSize + cellSize / 2,
                    Stroke = Brushes.Yellow,
                    StrokeThickness = 2
                };

                MazeCanvas.Children.Add(edgeLine);
            }
        }
    }
}



