using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using SaintCoinach;
using System.Threading;

namespace EorzeaVirtualAssistant
{
    public class DataBuilder
    {
        private string gameDir = string.Empty;
        private string dataDir = string.Empty;
        private readonly string cfgPath = "EVA.cfg";
        private readonly string dataBuilt = "data";
        private readonly string itemExt = ".txt";
        private string itemPath = string.Empty;

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

            //// DELETE THIS BEFORE PRODUCTION
            if (File.Exists(dataBuilt))          
            {
                File.Delete(dataBuilt);
            }   //// DELETE THIS BEFORE PRODUCTION

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
                File.Create(dataBuilt);
            }
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

        private void CreateEntry(StreamWriter fs, SaintCoinach.Xiv.Item item)
        {
            string[] itemStr = new string[item.SourceRow.Sheet.Header.ColumnCount];
            int itemStrIndex = 0;

            foreach (var col in item.SourceRow.Sheet.Header.Columns)
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
            finalizeWriteToFile(fs, itemStr);
        }

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

        //private void CreateEntry(StreamWriter fs, SaintCoinach.Xiv.Item item)
        //{
        //    string str = string.Empty;
        //    str += "\"AdditionalData\" : \"" + item.AdditionalData + "\"\n";
        //    //str += "\"Ask\" : \"" + item.Ask + "\"\n";
        //    str += "\"AsShopItems\" : \"" + GetAllShoppingList(item.AsShopItems) + "\"\n";
        //    str += "\"AsShopPayment\" : \"" + GetAllShoppingListPayment(item.AsShopPayment) + "\"\n";
        //    //str += "\"Bid\" : \"" + item.Bid + "\"\n";
        //    str += "\"CanBeHq\" : \"" + item.CanBeHq + "\"\n";
        //    str += "\"DefaultValue\" : \"" + item.DefaultValue + "\"\n";
        //    str += "\"Description\" : \"" + item.Description + "\"\n";
        //    str += "\"EquipSlotCategory\" : " + item.EquipSlotCategory + "\"\n";
        //    str += "\"GrandCompany\" : \"" + item.GrandCompany + "\"\n";
        //    str += "\"Icon\" : \"" + item.Icon + "\"\n";
        //    str += "\"IsAetherialReducible\" : \"" + item.IsAetherialReducible + "\"\n";
        //    str += "\"IsCollectable\" : \"" + item.IsCollectable + "\"\n";
        //    str += "\"IsConvertable\" : \"" + item.IsConvertable + "\"\n";
        //    str += "\"IsDyeable\" : \"" + item.IsDyeable + "\"\n";
        //    str += "\"IsGlamourous\" : \"" + item.IsGlamourous + "\"\n";
        //    str += "\"IsIndisposable\" : \"" + item.IsIndisposable + "\"\n";
        //    str += "\"IsUnique\" : \"" + item.IsUnique + "\"\n";
        //    str += "\"IsUntradable\" : \"" + item.IsUntradable + "\"\n";
        //    str += "\"ItemAction\" : \"" + item.ItemAction + "\"\n";
        //    str += "\"ItemLevel\" : \"" + item.ItemLevel + "\"\n";
        //    str += "\"ItemSearchCategory\" : \"" + item.ItemSearchCategory + "\"\n";
        //    str += "\"ItemUICategory\" : \"" + item.ItemUICategory + "\"\n";
        //    str += "\"Key\" : \"" + item.Key + "\"\n";
        //    str += "\"ModelMain\" : \"" + item.ModelMain + "\"\n";
        //    str += "\"ModelSub\" : \"" + item.ModelSub + "\"\n";
        //    str += "\"Name\" : \"" + item.Name + "\"\n";
        //    str += "\"Plural\" : \"" + item.Plural + "\"\n";
        //    str += "\"Rarity\" : \"" + item.Rarity + "\"\n";
        //    str += "\"RecipesAsMaterial\" : \"" + GetAllRecipe(item.RecipesAsMaterial) + "\"\n";
        //    str += "\"RecipesAsResult\" : \"" + GetAllRecipe(item.RecipesAsResult) + "\"\n";
        //    str += "\"RepairClassJob\" : \"" + item.RepairClassJob + "\"\n";
        //    str += "\"Sheet\" : \"" + item.Sheet + "\"\n";
        //    str += "\"Singular\" : \"" + item.Singular + "\"\n";
        //    str += "\"SourceRow\" : \"" + item.SourceRow + "\"\n";
        //    str += "\"Sources\" : \"" + GetAllSources(item.Sources) + "\"\n";
        //    str += "\"StackSize\" : \"" + item.StackSize + "\"\n";
        //    fs.Write(str);
        //    fs.Flush();
        //}

        private String GetAllSources(IEnumerable<SaintCoinach.Xiv.IItemSource> src)
        {
            String str = "{";
            foreach (SaintCoinach.Xiv.IItemSource index in src)
            {
                str += "\"";
                str += index;
                str += "\",";             
            }
            str = str.Remove(str.Length-1);
            str += "}";
            return str;
        }

        private String GetAllRecipe(IEnumerable<SaintCoinach.Xiv.Recipe> recipe)
        {
            String str = "{";

            foreach (var element in recipe)
            {
                str += "\"";
                str += element.Key;
                str += "\"";
            }
            str += "}";
            return str;
        }

        private String GetAllShoppingListPayment(IEnumerable<SaintCoinach.Xiv.IShopListingItem> sli)
        {
            String str = "{";
            foreach (SaintCoinach.Xiv.IShopListingItem index in sli)
            {
                str += "[";
                str += "\"";
                str += index.CollectabilityRating;
                str += "\",";
                str += "\"";
                str += index.Count;
                str += "\",";
                str += "\"";
                str += index.IsHq;
                str += "\",";
                str += "\"";
                str += index.Item;
                str += "\",";
                str += "\"";
                str += index.ShopItem;
                str += "\",";
                str += "]";
            }
            str += "}";
            return str;
        }

        private String GetAllShoppingList(IEnumerable<SaintCoinach.Xiv.IShopListing> sl)
        {
            String str = "{";
            foreach (SaintCoinach.Xiv.IShopListing index in sl)
            {
                foreach (var cost in index.Costs)
                {
                    str += "\"";
                    str += cost;
                    str += "\",";
                }
            }
            str += ";";
            foreach (SaintCoinach.Xiv.IShopListing index in sl)
            {
                foreach (var reward in index.Rewards)
                {
                    str += "\"";
                    str += reward;
                    str += "\"";
                }
            }
            str += ";";
            foreach (SaintCoinach.Xiv.IShopListing index in sl)
            {
                foreach (var shop in index.Shops)
                {
                    str += "\"";
                    str += shop.Name;
                    str += "\"";
                }
            }
            str += "}";
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
            string tempStr = string.Empty;
            bool gameDirAssigned = false;
            bool dataDirAssigned = false;
            do
            {
                tempStr = streamReader.ReadLine();
                if (tempStr[0] == '#')
                {
                }
                else if (!gameDirAssigned)
                {
                    this.gameDir = tempStr;
                    gameDirAssigned = true;
                }
                else if (!dataDirAssigned)
                {
                    this.dataDir = tempStr;
                    dataDirAssigned = true;
                }
                tempStr = string.Empty;
            }
            while (dataDirAssigned != true);
        }
    }

    
}
