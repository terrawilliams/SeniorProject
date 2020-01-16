using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.RandomTables
{
    class RandomTable
    {
        #region Constructors

        #endregion

        #region Members

        #endregion

    }


    class RandomTables_Gen : GeneralGenerator
    {
        #region Constructors

        RandomTables_Gen()
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
        RandomTable getTable(string tableName)
        {
            string query = "Select * From " + tableName + ";";

            

            return new RandomTable();
        }



        #endregion

    }
}
