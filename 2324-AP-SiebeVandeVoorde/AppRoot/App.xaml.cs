using DataAccessLayer;
using Globals.Interfaces;
using LogicLayer;
using PresentationLayer;
using System.Windows;

namespace AppRoot
{
    /// <summary>
    /// Composition root for App
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            IMazeDataAccess data = MazeDataAccessFactory.CreateMazeDataAccess();
            IBasicMazeGenerator basicMazeGenerator = BasicMazeGeneratorFactory.CreateBasicMazeGenerator(data);
            IAddWallMazeGenerator addWallMazeGenerator = AddWallMazeGeneratorFactory.CreateAddWallMazeGenerator();
            IRemoveWallMazeGenerator removeWallMazeGenerator = RemoveWallMazeGeneratorFactory.CreateRemoveWallMazeGenerator();
            new MainWindow(basicMazeGenerator, addWallMazeGenerator, removeWallMazeGenerator).Show();
        }
    }
}