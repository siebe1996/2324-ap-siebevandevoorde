using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globals.Interfaces;

namespace DataAccessLayer
{
    public class MazeDataAccessFactory
    {
        public static IMazeDataAccess CreateMazeDataAccess()
        {
            // You can choose different implementations based on your needs (e.g., CSV, database)
            return new CsvMazeDataAccess();
        }
    }
}
