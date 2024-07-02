using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LMS___Hunumulla_Central_Collage
{
    class _botRun
    {
        // Instantiate random number generator.  
        private static readonly Random _random = new Random();
        public static string path = Directory.GetCurrentDirectory() + @"\Data\_sys-cache.txt";
        // Generates a random number within a range.      
        public static int RandomNumber()
        {
            return _random.Next(2, 5);
        }

        public static string checkRobot()
        {
            string val = "";
            List<string> lines = File.ReadLines(path).ToList();
            foreach (string line in lines)
            {
                if (!line.Equals(""))
                {
                    val = "Alson";
                }
            }
            return val;
        }
    }
}
