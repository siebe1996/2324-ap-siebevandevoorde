﻿using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class RemoveWallMazeGeneratorFactory
    {
        public static IMazeGenerator CreateRemoveWallMazeGenerator()
        {
            return new RemoveWallMazeGenerator();
        }
    }
}
