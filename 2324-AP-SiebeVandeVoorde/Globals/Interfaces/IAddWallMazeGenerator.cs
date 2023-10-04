using Globals.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Interfaces
{
    public interface IAddWallMazeGenerator
    {
        Maze GenerateMaze(int width, int height, int wallThickness);
    }
}
