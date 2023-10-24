using DataAccessLayer;
using Globals.Interfaces;
using LogicLayer;
using PresentationLayer;
using System.Windows;

namespace AppRoot3DMaze
{
    // can use 3D tools, helixtoolkit
    /// <summary>
    /// Composition root for App
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            IMazeDataAccess data = MazeDataAccessFactory.CreateMazeDataAccess();
            IBasicMazeGenerator basicMazeGenerator = BasicMazeGeneratorFactory.CreateBasicMazeGenerator(data);
            IMazeGenerator addWallMazeGenerator = AddWallMazeGeneratorFactory.CreateAddWallMazeGenerator();
            IMazeGenerator removeWallMazeGenerator = RemoveWallMazeGeneratorFactory.CreateRemoveWallMazeGenerator();
            new MainWindow3D(removeWallMazeGenerator).Show();
        }
    }
}