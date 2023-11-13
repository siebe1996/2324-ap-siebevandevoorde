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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow3D.xaml
    /// </summary>
    public partial class MainWindow3D : Window
    {
        private readonly IMazeGenerator removeWallMazeGenerator;
        public MainWindow3D(IMazeGenerator removeWallMazeGenerator)
        {
            InitializeComponent();
            this.removeWallMazeGenerator = removeWallMazeGenerator;
            Maze maze = removeWallMazeGenerator.GenerateMaze(5, 5, 2);
            DrawMaze(maze);
        }

        private void DrawMaze(Maze maze)
        {
            var modelGroup = new Model3DGroup();

            double cellSize = 1.0;

            foreach (var vertex in maze.MazeGraph.Vertices)
            {

                bool isConnectedAbove = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, -1, 0));
                bool isConnectedRight = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, 1));
                bool isConnectedBelow = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 1, 0));
                bool isConnectedLeft = maze.MazeGraph.ContainsEdge(vertex, maze.GetNeighbor(vertex, 0, -1));

                double xPositionCenter = (vertex.Row * cellSize) + (cellSize / 2);
                double zPositionCenter = (vertex.Column * cellSize) + (cellSize / 2);

                var cellCube = new BoxVisual3D
                {
                    Width = cellSize/5,
                    Height = cellSize,
                    Length = cellSize,
                    Center = new Point3D(xPositionCenter, 0, zPositionCenter),
                    Material = Materials.Red,
                };

                var cellModel = cellCube.Model;
                modelGroup.Children.Add(cellCube.Content);

                if (!isConnectedAbove)
                {
                    var wallCube = new BoxVisual3D
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Length = cellSize / 20,
                        Material = Materials.Blue,
                        Center = new Point3D(vertex.Row * cellSize, cellSize / 2, zPositionCenter),
                    };

                    modelGroup.Children.Add(wallCube.Content);
                }

                if (!isConnectedRight)
                {
                    var wallCube = new BoxVisual3D
                    {
                        Width = cellSize,
                        Height = cellSize / 20,
                        Length = cellSize,
                        Material = Materials.Green,
                        Center = new Point3D(xPositionCenter, cellSize / 2, vertex.Column * cellSize + cellSize),
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
                        Center = new Point3D(vertex.Row * cellSize + cellSize, cellSize / 2, zPositionCenter),
                    };

                    modelGroup.Children.Add(wallCube.Content);
                }

                if (!isConnectedLeft)
                {
                    var wallCube = new BoxVisual3D
                    {
                        Width = cellSize,
                        Height = cellSize/ 20,
                        Length = cellSize,
                        Material = Materials.Violet,
                        Center = new Point3D(xPositionCenter, cellSize / 2, vertex.Column * cellSize),
                    };

                    modelGroup.Children.Add(wallCube.Content);
                }

            }

            var mazeVisualizer = new ModelVisual3D
            {
                Content = modelGroup,
            };

            helixViewport.Children.Add(mazeVisualizer);
        }
    }
}
