using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS___Hunumulla_Central_Collage
{
    class setup_data
    {
        public static string getDataBaseLocation()
        {
            string dataString = System.AppDomain.CurrentDomain.BaseDirectory;
            return dataString+ "System/";
        }

        public static string getDataBaseName()
        {
            return "_sysdd.db";
        }
    }
}
