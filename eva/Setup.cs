using System;
using System.IO;

namespace EorzeaVirtualAssistant
{
    public class Setup
    {
        public static string[] cfg =
        {
            "# Delete this file in order to re-run setup.",
            "# GameDir = ",
            "# DataDir = "
        };

        public Setup()
        {

        }

        public static void BeginSetup(string gamePath, string dataPath)
        {
            if (!Directory.Exists(gamePath))
            {
                throw new DirectoryNotFoundException("Game path not valid");
            }
            if (!Directory.Exists(dataPath))
            {
                try
                {
                    Directory.CreateDirectory(dataPath);

                }
                catch (Exception)
                {
                    throw new DirectoryNotFoundException("Data path not valid");
                }
            }
            if (File.Exists("EVA.cfg"))
            {
                File.Delete("EVA.cfg");
            }

            CreateCFG(gamePath, dataPath);
        }

        public static void CreateCFG(string gamePath, string dataPath)
        {
            StreamWriter fs = File.CreateText("EVA.cfg");
            fs.WriteLine(cfg[0]);
            fs.WriteLine(cfg[1]);
            fs.WriteLine(gamePath);
            fs.WriteLine(cfg[2]);
            fs.WriteLine(dataPath);
            fs.Flush();
        }
    }

}