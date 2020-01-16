using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//sql
using System.Data.SQLite;
//dataset
using System.Data;

namespace ChaoticCreation.RandomTables
{
    class RandomTableEntry
    {
        #region Constructors

        #endregion

        #region Members
        public int tableType { get; set; }
        public int lower { get; set; }
        public int upper { get; set; }
        public string description { get; set; }

        #endregion

    }


    class RandomTables_Gen : GeneralGenerator
    {
        #region Constructors

        public RandomTables_Gen()
        {
            tableList = new Dictionary<string, string>
            {
                {"25 GP Art Objects", "ArtObjects25Tbl" },
                {"250 GP Art Objects", "ArtObjects250Tbl" },
                {"750 GP Art Objects", "ArtObjects750Tbl" },
                {"2,500 GP Art Objects", "ArtObjects2500Tbl" },
                {"7,500 GP Art Objects", "ArtObjects7500Tbl" },
                {"10 GP Gemstones", "Gemstones10Tbl" },
                {"50 GP Gemstones", "Gemstones50Tbl" },
                {"100 GP Gemstones", "Gemstones100Tbl" },
                {"500 GP Gemstones", "Gemstones500Tbl" },
                {"1,000 GP Gemstones", "Gemstones1000Tbl" },
                {"5,000 GP Gemstones", "Gemstones5000Tbl" },
                {"Magic Item Table A" , "MagicItemsATbl"},
                {"Magic Item Table B", "MagicItemsBTbl" },
                {"Magic Item Table C", "MagicItemsCTbl" },
                {"Magic Item Table D", "MagicItemsDTbl"},
                {"Magic Item Table E", "MagicItemsETbl"},
                {"Magic Item Table F", "MagicItemsFTbl"},
                {"Magic Item Table G", "MagicItemsGTbl"},
                {"Magic Item Table H", "MagicItemsHTbl"},
                {"Magic Item Table I", "MagicItemsITbl"},
                {"Random Necromantic Effects", "NecromancyEffectsTbl" },
                {"Random Potion Effects", "PotionEffectsTbl" },
                {"Trinkets", "TrinketsTbl" },
                {"Wild Magic Surge", "WildMagicTbl"}
            };
        }



        #endregion

        #region Members
        Dictionary<string, string> tableList;

        #endregion

        #region Methods
        public List<RandomTableEntry> GetTable(string tableName)
        {
            string query = "Select * From " + tableList[tableName] + ";";

            DataSet data = QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", query);

            DataRowCollection rowValue = data.Tables[0].Rows;
            DataColumnCollection colValue = data.Tables[0].Columns;

            List<RandomTableEntry> newTable = new List<RandomTableEntry>(); 

            int i = 1;

            foreach (DataRow row in rowValue)
            {
                RandomTableEntry newEntry = new RandomTableEntry();

                Object[] rowData = row.ItemArray;

                newEntry.tableType = int.Parse(rowData[0].ToString());

                if(rowData.Length == 2)
                {
                    newEntry.lower = i;
                    newEntry.upper = i;
                    newEntry.description = rowData[1].ToString();
                }
                else
                {
                    newEntry.lower = int.Parse(rowData[1].ToString());
                    newEntry.upper = int.Parse(rowData[2].ToString());
                    newEntry.description = rowData[3].ToString();
                }

                newTable.Add(newEntry);
                
            }


            return newTable;
        }

        public RandomTableCategory InitializeRandomTableList()
        {
            RandomTableCategory listOfTables = new RandomTableCategory("Random Tables");
            listOfTables.SubCategories.Add(InitializeArtObjectsList());
            listOfTables.SubCategories.Add(InitializeGemstonesList());
            listOfTables.SubCategories.Add(InitializeMagicEffectsList());
            listOfTables.SubCategories.Add(InitializeMagicItemsList());
            listOfTables.SubCategories.Add(new RandomTableCategory("Trinkets"));

            return listOfTables;
        }

        private RandomTableCategory InitializeArtObjectsList()
        {
            RandomTableCategory artObjects = new RandomTableCategory("Art Objects");

            artObjects.SubCategories.Add(new RandomTableCategory("25 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("250 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("750 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("2,500 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("7,500 GP Art Objects"));

            return artObjects;
        }

        private RandomTableCategory InitializeGemstonesList()
        {
            RandomTableCategory gemstones = new RandomTableCategory("Gemstones");

            gemstones.SubCategories.Add(new RandomTableCategory("10 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("50 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("100 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("500 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("1,000 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("5,000 GP Gemstones"));

            return gemstones;
        }

        private RandomTableCategory InitializeMagicEffectsList()
        {
            RandomTableCategory magicEffects = new RandomTableCategory("Magic Effects");

            magicEffects.SubCategories.Add(new RandomTableCategory("Potions"));
            magicEffects.SubCategories.Add(new RandomTableCategory("Necromancy"));
            magicEffects.SubCategories.Add(new RandomTableCategory("Wild Magic Surge"));

            return magicEffects;
        }

        private RandomTableCategory InitializeMagicItemsList()
        {
            RandomTableCategory magicItems = new RandomTableCategory("Magic Items");

            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table A"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table B"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table C"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table D"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table E"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table F"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table G"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table H"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table I"));

            return magicItems;
        }



        #endregion

    }
}
