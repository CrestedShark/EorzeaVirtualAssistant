using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flurl;
using Flurl.Http;
using SaintCoinach;
using System.Data.SqlServerCe;

namespace EorzeaVirtualAssistant
{
    public class DataBuilder
    {
        private readonly string cfgPath = Toolbox.Instance.StringTable["cfgPath"];
        private readonly string dataBuilt = Toolbox.Instance.StringTable["dataBuilt"];
        private readonly string itemExt = Toolbox.Instance.StringTable["itemExt"];
        private string gameDir = string.Empty;
        private string dataDir = string.Empty;
        private string itemPath = string.Empty;
        Dictionary<int, int> sizeVariations = new Dictionary<int, int>();  //// DELETE THIS BEFORE PRODUCTION

        public DataBuilder()
        {
            

            FileStream setupFS;
            if (File.Exists(cfgPath))
            {
                setupFS = File.OpenRead(cfgPath);
            }
            else
            {
                throw new FileNotFoundException(cfgPath + " not found");
            }

            EstablishPaths(setupFS);
            
            if (gameDir.Length > 0)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                ParseLocalData();
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
            }
            else
            {
                throw new DirectoryNotFoundException("game dir not specified");
            }
        }

        public void ParseLocalData()
        {                                           // extract data from local FFXIV installation
            int itemCount = 0;
            var realm = new SaintCoinach.ARealmReversed(gameDir, SaintCoinach.Ex.Language.English);
            if (!realm.IsCurrentVersion)
            {
                const bool IncludeDataChanges = true;
                var updateReport = realm.Update(IncludeDataChanges);
                Console.WriteLine(updateReport.Changes.Count + " files updated.");
                if (File.Exists(dataBuilt))
                {
                    File.Delete(dataBuilt);
                }
            }

            //// DELETE THIS BEFORE PRODUCTION     //////
            if (File.Exists(dataBuilt))                //
            {                                          //
                File.Delete(dataBuilt);                //
            }                                          //
            if (File.Exists(dataBuilt))                //
            {                                          //
                File.Delete(dataBuilt);                //
            }                                          //
            //// DELETE THIS BEFORE PRODUCTION     //////

            if (!File.Exists(dataBuilt))
            {
                CreateOrEmptyDirectories();
                List<string> itemData = new List<string>();
                itemCount = realm.GameData.GetSheet<SaintCoinach.Xiv.Item>().Count;

                var itemList = realm.GameData.GetSheet<SaintCoinach.Xiv.Item>();

                //for (int ix = 1; ix <= 10; ix++)
                foreach (var item in itemList)
                {
                    string itemName = item.Name;
                    itemName = RemovePathUnfriendlyCharacters(itemName);
                    string path = itemPath + itemName + itemExt;
                    //string path = itemPath + item.Name + itemExt;
                    if (!File.Exists(path))
                    {
                        CreateEntry(File.CreateText(path), item);
                    }
                }
                //// DELETE THIS BEFORE PRODUCTION              //////
                string devSizeFile = "sizes" + itemExt;             //
                if (true)                      //
                {                                                   //
                    SaveSizesToFile(File.CreateText(devSizeFile));  //
                }                                                   //
                //// DELETE THIS BEFORE PRODUCTION              //////
                File.Create(dataBuilt);
            }
        }

        private void CreateEntry(StreamWriter fs, SaintCoinach.Xiv.Item item)
        {
            string[] itemStr = new string[item.SourceRow.Sheet.Header.ColumnCount];
            int itemStrIndex = 0;
            var AllColumns = item.SourceRow.Sheet.Header.Columns;

            foreach (var col in AllColumns)
            {
                // insert col key, {"name"} :
                itemStr[itemStrIndex] += WrapStringInBrackets(WrapStringInQuotes(col.Name));
                itemStr[itemStrIndex] += ":";

                // insert col key, {"name"} :
                var colRaw = item.ColumnValues().ElementAt(itemStrIndex);
                if (colRaw == null)
                {
                    itemStr[itemStrIndex] += WrapStringInBrackets("null");
                }
                else if (colRaw.GetType().Equals(typeof(SaintCoinach.Text.XivString)))
                {
                    // insert col child(ren), {"name"} : {"Water Crystal"}
                    SaintCoinach.Text.XivString colVal = (SaintCoinach.Text.XivString)colRaw;
                    itemStr[itemStrIndex] += CreateFormattedStringWithBracketEnclosement(colVal.Children);
                }
                else
                {
                    itemStr[itemStrIndex] += WrapStringInBrackets(WrapStringInQuotes(colRaw.ToString()));
                }
                itemStrIndex++;
            }
                
            captureSize(AllColumns.Count());    //// DELETE THIS BEFORE PRODUCTION

            finalizeWriteToFile(fs, itemStr);
        }

        public string RemovePathUnfriendlyCharacters(string src)
        {
            string formatted = string.Empty;
            var invalidCharArr = Path.GetInvalidPathChars();
            bool stringValid = true;
            foreach (char c in src)
            {
                stringValid = true;
                foreach (char invalidChar in invalidCharArr)
                {
                    if (c == invalidChar || c == ':' || c == '\\' || c == '/')
                    {
                        stringValid = false;
                        formatted += "-";
                        break;
                    }
                }
                if (stringValid)
                {
                    formatted += c;
                }
            }
            return formatted;
        }

        private void SaveSizesToFile(StreamWriter fs)
        {
            List<string> strs = new List<string>();
            foreach (KeyValuePair<int,int> kv in sizeVariations)
            {
                strs.Add(kv.Key.ToString() + ": " + kv.Value.ToString());
            }
            finalizeWriteToFile(fs, strs.ToArray());
        }

        //// DELETE THIS BEFORE PRODUCTION                //////
        private void captureSize(int size)                    //
        {                                                     //
            if (sizeVariations.ContainsKey(size))             //
            {                                                 //
                ++sizeVariations[size];                       //
            }                                                 //
            else                                              //
            {                                                 //
                sizeVariations.Add(size, 1);                  //
            }                                                 //
        }                                                     //
        //// DELETE THIS BEFORE PRODUCTION                //////

        private void finalizeWriteToFile(StreamWriter fs, string[] itemStr)
        {
            foreach (string str in itemStr)
            {
                fs.WriteLine(str);
            }
            fs.Flush();
        }

        private string CreateFormattedStringWithBracketEnclosement(IEnumerable<object> values)
        {
            string str = string.Empty;
            int numElementsTraversed = 0;
            foreach (object element in values)
            {
                str += WrapStringInQuotes(element.ToString());
                numElementsTraversed++;
                if (numElementsTraversed < values.Count())
                {
                    str += ",";
                }
            }
            return WrapStringInBrackets(str);
        }

        private string WrapStringInBrackets(string value)
        {
            string str = string.Empty;
            str += "{" + value + "}";
            return str;
        }

        private string WrapStringInQuotes(string value)
        {
            string str = string.Empty;
            str += "\"" + value + "\"";
            return str;
        }


        private void CreateOrEmptyDirectories()
        {
            itemPath = this.dataDir + "Items\\";
            if (!Directory.Exists(itemPath))
            {
                Directory.CreateDirectory(itemPath);
            }
        }

        public void ParseOnlineData()
        {                                   // request data from xivapi.com
        }


        public void EstablishPaths(FileStream fs)
        {
            StreamReader streamReader = new StreamReader(fs);
            string tempStr;
            bool done = false;
            bool gameDirAssigned = false;
            do
            {
                tempStr = streamReader.ReadLine();
                if (tempStr[0] == '#')
                {
                }
                else if (!gameDirAssigned)
                {
                    gameDir = Toolbox.Instance.StringTable[gameDir] = tempStr;
                    gameDirAssigned = true;
                }
                else
                {
                    dataDir = Toolbox.Instance.StringTable[dataDir] = tempStr;
                    done = true;
                }
            }
            while (!done);
            Toolbox.Instance.StringTable["itemPath"] = this.dataDir + "Items\\";
        }
    }

    
}
