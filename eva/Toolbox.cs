using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EorzeaVirtualAssistant
{
    public class Toolbox : Singleton
    {
        public SqlConnection activeDB = null;
        public Dictionary<String, String> StringTable = null;

        public Toolbox()
        {
            PopulateStringTable();
        }   

        private void PopulateStringTable()
        {
            StringTable = new Dictionary<String, String>()
            {
                { "cfgPath", "EVA.cfg" },
                { "dataBuilt", "data" },
                { "itemExt", ".txt" },
                { "gameDir", string.Empty },
                { "dataDir", string.Empty },
                { "itemPath", string.Empty },
                { "connectString", @"Server=localhost\SQLEXPRESS;Database=testdb;Trusted_Connection=True;" }
            };
        }

    }

    //layout from https://csharpindepth.com/Articles/Singleton
    public class Singleton
    {
        private static readonly object padlock = new object();
        private static Toolbox instance = default;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Singleton() { }
        public Singleton() { }

        public static Toolbox Instance
        {

            get
            {
                lock (padlock)
                {
                    if (instance == default)
                    {
                        instance = new Toolbox();
                    }

                    return instance;
                }
            }
        }
    }
}