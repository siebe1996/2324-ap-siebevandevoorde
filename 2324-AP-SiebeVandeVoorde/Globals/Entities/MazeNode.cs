using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Entities
{
    public class MazeNode
    {
        public int Row { get; }
        public int Column { get; }
        public char Value { get; set; }

        public MazeNode(int row, int column, char value)
        {
            Row = row;
            Column = column;
            Value = value;
        }
    }

}
