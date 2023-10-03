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
            IMazeGenerator logic = MazeGeneratorFactory.CreateMazeGenerator(data);
            new MainWindow(logic).Show();
        }
    }
}