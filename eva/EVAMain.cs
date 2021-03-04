using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

//============================================================================
// build local database of items via https://xivapi.com/ while minimizing data
// usage via https://github.com/ufx/SaintCoinach

namespace EorzeaVirtualAssistant
{
    static class EVAMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!File.Exists("EVA.cfg"))
            { 
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new EVAInitPopup());
            }

            var db = new DataBuilder(); // estimated runtime of 163 hours if all 33201 items generated... optimize this

            Application.Run(new EVAClient());
            //DataBuilder;
        }
    }
}
