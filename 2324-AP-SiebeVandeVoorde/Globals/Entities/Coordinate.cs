using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Entities
{
    public struct Coordinate
    {
        public double X { get; }
        public double Y { get; }
        public double? Z { get; }

        public Coordinate(double x, double y, double? z = null)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
    }
}
