using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Interfaces
{
    public interface IMazeGenerator
    {
        char[,] GenerateMaze(string filePath);
    }
}

