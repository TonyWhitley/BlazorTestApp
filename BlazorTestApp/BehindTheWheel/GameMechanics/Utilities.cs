using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace BehindTheWheel.GameMechanics
{
    public static class Utilities
    {
        private static Random rnd = new Random();
        public static int GetRandom(int min, int max)
        {
            return rnd.Next(min, max);
        }
        public static List<int> GetRandomOrder(int min, int max)
        {
            List<int> order = new List<int>();
            for (var i = min; i < max; i++)
            {
                var item = rnd.Next(min, max);
                while (order.Contains(item))
                {
                    item = rnd.Next(min, max);
                }
                order.Add(item);
            }
            return order;
        }

        static bool UseDebugFilePaths = System.Diagnostics.Debugger.IsAttached;
        public static String getDefaultLocation(String name, Boolean isFile = true)
        {
            string ApplicationStartupPath = @"d:\Tony\repos\BehindTheWheel\BlazorTestApp\BlazorTestApp\BehindTheWheel";
            String regularPath = ApplicationStartupPath + @"\" + name;
            String debugPath = ApplicationStartupPath + @"\..\..\..\" + name;
            // Unit tests path:
            String utPath = Directory.GetCurrentDirectory() + @"\..\..\..\BehindTheWheel\" + name;
            if (UseDebugFilePaths)
            {
                if (isFile)
                {
                    return File.Exists(debugPath) ? debugPath : File.Exists(regularPath) ? regularPath : utPath;
                }
                else
                {
                    return Directory.Exists(debugPath) ? debugPath : Directory.Exists(regularPath) ? regularPath : utPath;
                }
            }
            else
            {
                if (isFile)
                {
                    return File.Exists(regularPath) ? regularPath : File.Exists(debugPath) ? debugPath : utPath;
                }
                else
                {
                    return Directory.Exists(regularPath) ? regularPath : Directory.Exists(debugPath) ? debugPath : utPath;
                }
            }
        }
    }
}
