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
        private string cfgPath = "EVA.cfg";
        private string dataBuilt = "data";
        private string itemExt = ".txt";
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

            if (!File.Exists(dataBuilt))
            {
                CreateOrEmptyDirectories();
                List<string> itemData = new List<string>();
                itemCount = realm.GameData.GetSheet<SaintCoinach.Xiv.Item>().Count;
                var itemList = realm.GameData.GetSheet<SaintCoinach.Xiv.Item>();
                //foreach (var item in itemList)
                for (int ix = 1; ix <= itemCount; ix++)
                {
                    string path = itemPath + itemList[ix].Name + itemExt;
                    //string path = itemPath + item.Name + itemExt;
                    if (!File.Exists(path))
                    {
                        CreateEntry(File.CreateText(path), itemList[ix]);
                        //CreateEntry(File.CreateText(path), item);
                    }
                }
            }
        }

        private void CreateEntry(StreamWriter fs, SaintCoinach.Xiv.Item item)
        {
            fs.WriteLine("\"AdditionalData\" : \"" + item.AdditionalData + "\"");
            //fs.WriteLine("\"Ask\" : \"" + item.Ask + "\"");
            fs.WriteLine("\"AsShopItems\" : \"" + GetAllShoppingList(item.AsShopItems) + "\"");
            fs.WriteLine("\"AsShopPayment\" : \"" + GetAllShoppingListPayment(item.AsShopPayment) + "\"");
            //fs.WriteLine("\"Bid\" : \"" + item.Bid + "\"");
            fs.WriteLine("\"CanBeHq\" : \"" + item.CanBeHq + "\"");
            fs.WriteLine("\"DefaultValue\" : \"" + item.DefaultValue + "\"");
            fs.WriteLine("\"Description\" : \"" + item.Description + "\"");
            fs.WriteLine("\"EquipSlotCategory\" : " + item.EquipSlotCategory + "\"");
            fs.WriteLine("\"GrandCompany\" : \"" + item.GrandCompany + "\"");
            fs.WriteLine("\"Icon\" : \"" + item.Icon + "\"");
            fs.WriteLine("\"IsAetherialReducible\" : \"" + item.IsAetherialReducible + "\"");
            fs.WriteLine("\"IsCollectable\" : \"" + item.IsCollectable + "\"");
            fs.WriteLine("\"IsConvertable\" : \"" + item.IsConvertable + "\"");
            fs.WriteLine("\"IsDyeable\" : \"" + item.IsDyeable + "\"");
            fs.WriteLine("\"IsGlamourous\" : \"" + item.IsGlamourous + "\"");
            fs.WriteLine("\"IsIndisposable\" : \"" + item.IsIndisposable + "\"");
            fs.WriteLine("\"IsUnique\" : \"" + item.IsUnique + "\"");
            fs.WriteLine("\"IsUntradable\" : \"" + item.IsUntradable + "\"");
            fs.WriteLine("\"ItemAction\" : \"" + item.ItemAction + "\"");
            fs.WriteLine("\"ItemLevel\" : \"" + item.ItemLevel + "\"");
            fs.WriteLine("\"ItemSearchCategory\" : \"" + item.ItemSearchCategory + "\"");
            fs.WriteLine("\"ItemUICategory\" : \"" + item.ItemUICategory + "\"");
            fs.WriteLine("\"Key\" : \"" + item.Key + "\"");
            fs.WriteLine("\"ModelMain\" : \"" + item.ModelMain + "\"");
            fs.WriteLine("\"ModelSub\" : \"" + item.ModelSub + "\"");
            fs.WriteLine("\"Name\" : \"" + item.Name + "\"");
            fs.WriteLine("\"Plural\" : \"" + item.Plural + "\"");
            fs.WriteLine("\"Rarity\" : \"" + item.Rarity + "\"");
            fs.WriteLine("\"RecipesAsMaterial\" : \"" + GetAllRecipe(item.RecipesAsMaterial) + "\"");
            fs.WriteLine("\"RecipesAsResult\" : \"" + GetAllRecipe(item.RecipesAsResult) + "\"");
            fs.WriteLine("\"RepairClassJob\" : \"" + item.RepairClassJob + "\"");
            fs.WriteLine("\"Sheet\" : \"" + item.Sheet + "\"");
            fs.WriteLine("\"Singular\" : \"" + item.Singular + "\"");
            fs.WriteLine("\"SourceRow\" : \"" + item.SourceRow + "\"");
            fs.WriteLine("\"Sources\" : \"" + GetAllSources(item.Sources) + "\"");
            fs.WriteLine("\"StackSize\" : \"" + item.StackSize + "\"");
            fs.Flush();
        }

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
