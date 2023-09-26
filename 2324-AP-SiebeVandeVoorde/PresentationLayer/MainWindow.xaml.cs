using Globals.Interfaces;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogic _logic;

        public MainWindow(ILogic logic)
        {
            _logic = logic;
            InitializeComponent();
        }
    }
}