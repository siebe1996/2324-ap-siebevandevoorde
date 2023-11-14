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
                    tiltAngleX -= tiltSpeedX;
                    break;
                case Key.Right:
                    tiltAngleX += tiltSpeedX;
                    break;
                case Key.Up:
                    tiltAngleY -= tiltSpeedY;
                    break;
                case Key.Down:
                    tiltAngleY += tiltSpeedY;
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
            double gravityY = Math.Tan(tiltRadiansY); // Gravity in the Y direction

            double newX = (double)Math.Round(gravityX);
            double newY = (double)Math.Round(gravityY);
            double newZ = 0 + ball.Straal;

            /*((TranslateTransform3D)sphere.Transform).OffsetX = 10;
            ((TranslateTransform3D)sphere.Transform).OffsetY = 10;
            ((TranslateTransform3D)sphere.Transform).OffsetZ = 0;*/

            var oldX = sphere.Center.X;
            var oldY = sphere.Center.Y;

            //sphere.Transform = new TranslateTransform3D(10, 10, 0); //transform doesnt work???
            sphere.Center = new Point3D(oldX + gravityX, oldY + gravityY, newZ);
            
            SphereVisual3D sphereCjheck = sphere;

            // Update the TranslateTransform3D offsets based on the new ball position
            /*ballTransform.OffsetX = newX * cellSize;
            ballTransform.OffsetY = newY * cellSize;
            ballTransform.OffsetZ = newZ;*/
            //UpdateViewport();
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

                var cellCube = new BoxVisual3D
                {
                    Width = cellSize, //y-as groen right
                    Height = cellSize / 5, //z-as blauw up
                    Length = cellSize, // x-as rood towards me
                    Center = new Point3D(xPositionCenter, yPositionCenter, 0),
                    Material = Materials.Red,
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
