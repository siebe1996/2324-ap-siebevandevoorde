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
            //CreateMaze();
            this.removeWallMazeGenerator = removeWallMazeGenerator;
            var cube = new BoxVisual3D
            {
                Center = new System.Windows.Media.Media3D.Point3D(0, 0, 0),
                Width = 1,
                Length = 1,
                Height = 1,
                Fill = System.Windows.Media.Brushes.Blue
            };


            // Add the 3D content to the viewport
            helixViewport.Children.Add(cube);
        }

        
    }
}
