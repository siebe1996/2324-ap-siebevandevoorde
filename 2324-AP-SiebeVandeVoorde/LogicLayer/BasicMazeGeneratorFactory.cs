using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globals.Interfaces;

namespace LogicLayer
{
    public class BasicMazeGeneratorFactory
    {
        public static IBasicMazeGenerator CreateBasicMazeGenerator(IMazeDataAccess dataAccess)
        {
            // Pass the dataAccess dependency when creating BasicMazeGenerator
            return new BasicMazeGenerator(dataAccess);
        }
    }
}

