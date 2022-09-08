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
