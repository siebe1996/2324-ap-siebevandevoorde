using Globals.Entities;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Interfaces
{
    public interface IBasicMazeGenerator
    {
        /*char[,] GenerateMaze(string filePath);*/
        Maze GenerateGraphMaze(string filePath, int wallThickness);
    }
}


