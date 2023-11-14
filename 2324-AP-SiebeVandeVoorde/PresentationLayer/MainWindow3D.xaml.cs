using Globals.Entities;
using Globals.Interfaces;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.Windows.Threading;
using System.Data.Common;
using QuickGraph;

namespace PresentationLayer
{
    public partial class MainWindow3D : Window
    {
        private readonly IMazeGenerator removeWallMazeGenerator;
        private readonly Model3DGroup modelGroup;
        private double cellSize = 1.0;
        private Maze maze;
        private Ball ball;
        private SphereVisual3D sphere;

        private readonly DispatcherTimer updateTimer;
        private double gravitySpeed = 0.1;
        private double tiltAngleX = 0.0;
        private double tiltAngleY = 0.0;
        private double tiltSpeedX = 1.0;
        private double tiltSpeedY = 1.0;

        public MainWindow3D(IMazeGenerator removeWallMazeGenerator)
        {
            InitializeComponent();

            this.removeWallMazeGenerator = removeWallMazeGenerator;
            modelGroup = new Model3DGroup();

            updateTimer = new DispatcherTimer();
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);

            InitializeMaze();

            SetupInputHandlers();
        }

        private void InitializeMaze()
        {
            this.maze = removeWallMazeGenerator.GenerateMaze(5, 5, 2);
            this.ball = new Ball(cellSize / 4);
            this.maze.Ball = ball;
            DrawMaze();


            StartGravity();
        }

        private void SetupInputHandlers()
        {
            this.KeyUp += MainWindow3D_KeyUp;
        }

        private void MainWindow3D_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    tiltAngleY -= tiltSpeedX;
                    break;
                case Key.Right:
                    tiltAngleY += tiltSpeedX;
                    break;
                case Key.Up:
                    tiltAngleX -= tiltSpeedY;
                    break;
                case Key.Down:
                    tiltAngleX += tiltSpeedY;
                    break;
            }
        }

        private void StartGravity()
        {
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            ApplyGravity();
        }

        private void ApplyGravity()
        {


            double tiltRadiansX = tiltAngleX * Math.PI / 180.0;
            double tiltRadiansY = tiltAngleY * Math.PI / 180.0;
            double gravityX = Math.Sin(tiltRadiansX);
            double gravityY = Math.Tan(tiltRadiansY);

            double newZ = 0 + ball.Straal;

            var oldX = sphere.Center.X;
            var oldY = sphere.Center.Y;

            if (!CheckWallCollision(oldX + gravityX, oldY + gravityY))
            {
                sphere.Center = new Point3D(oldX + gravityX, oldY + gravityY, newZ);
            }
        }

        private bool CheckWallCollision(double x, double y)
        {
            int column = (int)Math.Floor(y);
            int row = (int)Math.Floor(x);
            var vertex = maze.MazeGraph.Vertices.FirstOrDefault(v => v.Column == column && v.Row == row);
            bool isConnectedAbove = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, -1, 0));
            bool isConnectedRight = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, 1));
            bool isConnectedBelow = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 1, 0));
            bool isConnectedLeft = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, -1));

            double wallLeft = vertex.Column * cellSize;
            double wallRight = (vertex.Column + 1) * cellSize;
            double wallTop = vertex.Row * cellSize;
            double wallBottom = (vertex.Row + 1) * cellSize;

            if (!isConnectedAbove && x - ball.Straal < wallTop)
            {
                return true; // botsing met top muur
            }

            if (!isConnectedRight && y + ball.Straal > wallRight)
            {
                return true;
            }

            if (!isConnectedBelow && x + ball.Straal > wallBottom)
            {
                return true;
            }

            if (!isConnectedLeft && y - ball.Straal < wallLeft)
            {
                return true;
            }

            return false; // Geen botsing
        }



        private void DrawMaze()
        {
            modelGroup.Children.Clear();

            foreach (var vertex in maze.MazeGraph.Vertices)
            {

                bool isConnectedAbove = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, -1, 0));
                bool isConnectedRight = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, 1));
                bool isConnectedBelow = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 1, 0));
                bool isConnectedLeft = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, -1));

                double xPositionCenter = (vertex.Row * cellSize) + (cellSize / 2);
                double yPositionCenter = (vertex.Column * cellSize) + (cellSize / 2);

                var test = Materials.Red;
                if (vertex.Row == 0 && vertex.Column == 1)
                {
                    test = Materials.Gold;
                }
             
                var cellCube = new BoxVisual3D
                {
                    Width = cellSize, //y-as groen right
                    Height = cellSize / 5, //z-as blauw up
                    Length = cellSize, // x-as rood towards me
                    Center = new Point3D(xPositionCenter, yPositionCenter, 0),
                    Material = test,
                };

                modelGroup.Children.Add(cellCube.Content);

                if (!isConnectedAbove)
                {
                    var wallCube = new BoxVisual3D
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Length = cellSize / 20,
                        Material = Materials.Blue,
                        Center = new Point3D(vertex.Row * cellSize, yPositionCenter, cellSize / 2),
                    };

                    modelGroup.Children.Add(wallCube.Content);
                }

                if (!isConnectedRight)
                {
                    var wallCube = new BoxVisual3D
                    {
                        Width = cellSize / 20,
                        Height = cellSize,
                        Length = cellSize,
                        Material = Materials.Green,
                        Center = new Point3D(xPositionCenter, vertex.Column * cellSize + cellSize, cellSize / 2),
                    };

                    modelGroup.Children.Add(wallCube.Content);
                }

                if (!isConnectedBelow)
                {
                    var wallCube = new BoxVisual3D
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Length = cellSize / 20,
                        Material = Materials.Orange,
                        Center = new Point3D(vertex.Row * cellSize + cellSize, yPositionCenter, cellSize / 2),
                    };

                    modelGroup.Children.Add(wallCube.Content);
                }

                if (!isConnectedLeft)
                {
                    var wallCube = new BoxVisual3D
                    {
                        Width = cellSize / 20,
                        Height = cellSize,
                        Length = cellSize,
                        Material = Materials.Violet,
                        Center = new Point3D(xPositionCenter, vertex.Column * cellSize, cellSize / 2),
                    };

                    modelGroup.Children.Add(wallCube.Content);
                }

                if (vertex.Row == maze.BallPosition.X && vertex.Column == maze.BallPosition.Y)
                {
                    DrawBall(xPositionCenter, yPositionCenter);
                }

            }
            UpdateViewport();
        }

        private void DrawBall(double xPosition, double yPosition)
        {
            var ballTransform = new TranslateTransform3D(xPosition, yPosition, cellSize/2); //transform doesnt work???

            sphere = new SphereVisual3D
            {
                Radius = ball.Straal,
                Material = Materials.Yellow,
                Center = new Point3D(xPosition, yPosition, cellSize/2),
                Transform = ballTransform,
            };

            modelGroup.Children.Add(sphere.Content);
        }

        private void UpdateViewport()
        {
            var mazeVisualizer = new ModelVisual3D
            {
                Content = modelGroup,
            };

            helixViewport.Children.Add(mazeVisualizer);
        }
    }
}
