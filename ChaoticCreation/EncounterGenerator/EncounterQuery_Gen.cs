using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;

namespace ChaoticCreation.EncounterGenerator
{
    class EncounterQuery_Gen : GeneralGenerator
    {        
        #region Members        
        #endregion

        #region Methods
        public Dictionary<string, Dictionary<string, string>> EncounterQuery(List<string> userSpecifiedData)
        {
            //userSpecifiedData[0] = size, [1] = level, etc
            string partySize = userSpecifiedData[0];
            string partyLevel = userSpecifiedData[1];
            string terrain = userSpecifiedData[2];
            string difficulty = userSpecifiedData[3];

            Console.WriteLine("USER SPECIFIED DATA:");
            Console.WriteLine("partySize = " + partySize);
            Console.WriteLine("partyLevel = " + partyLevel);
            Console.WriteLine("terrain = " + terrain);
            Console.WriteLine("difficulty = " + difficulty);

            //Encounter Calculation
            int partyXP = calcTotalXP(partySize, partyLevel, difficulty); //Returns party XP Threshold
            int monsterNum = getMonsterNum(); //Returns random number of monsters
            double monsterMultiplier = getMonsterMultiplier(monsterNum); //Returns multiplier based on number of monsters
            int monsterTotalXp = Convert.ToInt32(partyXP / monsterMultiplier); //Total XP for all monsters

            Console.WriteLine("VALUES");
            Console.WriteLine("partyXP = " + partyXP);
            Console.WriteLine("monsterNum = " + monsterNum);
            Console.WriteLine("monsterMultiplier = " + monsterMultiplier);
            Console.WriteLine("monsterTotalXp = " + monsterTotalXp);

            if (monsterTotalXp < 10) { monsterTotalXp = 10; }

            int eachMonstXp = monsterTotalXp/monsterNum;
            Console.WriteLine("eachMonstXp = " + eachMonstXp);
            float eachMonstCr = getCR(eachMonstXp);

            //No monsters with CR = 28 or 29 in DB
            if(eachMonstCr == 29) { eachMonstCr = 30; }
            if (eachMonstCr == 28) { eachMonstCr = 27; }
            Console.WriteLine("CR = " + eachMonstCr);

            string queryString = "SELECT monsterName, cr FROM monsterTbl WHERE cr == " + eachMonstCr +
                " AND environment LIKE '%" + terrain + "%' ORDER BY cr;";

            //Returns all monsters below total XP threshold
            Dictionary<string, float> queryResult = QueryMonsterTbl("..\\..\\sqlDatabase\\MasterDB.db", queryString);
            
            //If the query result is empty, remove terrain
            if (queryResult.Count() == 0)
            {
                queryString = "SELECT monsterName, cr FROM monsterTbl WHERE cr == " + eachMonstCr + 
                    " AND environment IS Null ORDER BY cr;";
                queryResult = QueryMonsterTbl("..\\..\\sqlDatabase\\MasterDB.db", queryString);
            }            

            //Returns list of monsters for cr level
            List<KeyValuePair<string, float>> tempDictionary = SelectMonsters(queryResult, monsterNum, monsterTotalXp);
            
            //Returns Dictionary<monster name, amt of that monster>
            Dictionary<string, string> generatedEncounter = CombineMonsters(tempDictionary);
            
            Console.WriteLine("Generated Encounter");
            foreach (KeyValuePair<string, string> item in generatedEncounter)
            {Console.WriteLine("Key: " + item.Key + " Value: " + item.Value);}

            //float cr = getCR(monsterTotalXp);
            
            Dictionary<string, string> lootDict = lootCalc(eachMonstCr, monsterNum, generatedEncounter);

            /*
            Console.WriteLine("CALCULATED LOOT");
            foreach(KeyValuePair<string,string> item in lootDict)
            { Console.WriteLine("Key: " + item.Key + " Value: " + item.Value); }
            */

            Dictionary<string, Dictionary<string, string>> monstersAndLoot = new Dictionary<string, Dictionary<string, string>>();

            monstersAndLoot.Add("Monsters", generatedEncounter);
            monstersAndLoot.Add("Loot", lootDict);

            return monstersAndLoot;
        }
        private Dictionary<string, string> lootCalc(float CR, int monsterNum, Dictionary<string, string> monsterList)
        {
            Dictionary<string, int> coinDict = new Dictionary<string, int>(); //[0: CP, 1:SP, 2:EP, 3:GP, 4: PP]
            Dictionary<string, string> hoardDict = new Dictionary<string, string>(); //[0: Gem/Art, 1:Magic]
            int[] coinArr = new int[5] { 0, 0, 0, 0, 0 };

            if (CR < 1)
            {
                CR = 0;
            }
            
            if (monsterNum == 1)
            {
                hoardDict = CalcHoard(CR);
                coinDict = CoinQuery(CR, true);
                for (int i = 0; i < 5; i++)
                {
                    //Console.WriteLine("coinArr[" + i + " ] = " + coinArr[i]);
                    coinArr[i] += coinDict.ElementAt(i).Value;
                }
            }
            
            else if(monsterNum > 1){
                //Sort Monster List by CR
                Dictionary<string, int> sortedList = SortMonsterList(monsterList);
                int count = sortedList.Count;
                int highest = sortedList.ElementAt(0).Value;
                
                //hoardDict for highestCR
                hoardDict = CalcHoard(highest);
                coinDict = CoinQuery(highest, true);

                //For remaining monsters
                monsterNum--;
                Console.WriteLine("monster: " + sortedList.ElementAt(0).Key);
                for (int i = 0; i < 5; i++)
                {
                    coinArr[i] += coinDict.ElementAt(i).Value;
                    //Console.WriteLine("coinArr[" + i + " ] = " + coinArr[i]);
                }

                for (int j = 0; j<monsterNum; j++)
                {
                    coinDict = CoinQuery(CR, false);
                    for (int i = 0; i < 5; i++)
                    {
                        coinArr[i] += coinDict.ElementAt(i).Value;
                    }
                }
            }
            
            //[0: CP, 1:SP, 2:EP, 3:GP, 4: PP, 5:Gem/Art, 6:Magic]
            Dictionary<string, string> lootDict = new Dictionary<string, string>(); 
            
            lootDict.Add("CP", coinArr[0].ToString());
            lootDict.Add("SP", coinArr[1].ToString());
            lootDict.Add("EP", coinArr[2].ToString());
            lootDict.Add("GP", coinArr[3].ToString());
            lootDict.Add("PP", coinArr[4].ToString());
            lootDict.Add("Object", hoardDict["object"]);
            lootDict.Add("Magic", hoardDict["magic"]);

            return lootDict;
        }
        Dictionary<string, int> SortMonsterList(Dictionary<string, string> monsterList)
        {
            Dictionary<string, int> sortedList = new Dictionary<string, int>();
            foreach (KeyValuePair<string, string> monster in monsterList.OrderByDescending(key => key.Value))
            {
                sortedList.Add(monster.Key, Int32.Parse(monster.Value));
            }
            return sortedList;
        }
        private Dictionary<string, string> CalcHoard(float CR)
        {
            Dictionary<string, string> hoardDict = new Dictionary<string, string>();
            Dictionary<string, string> queryStrDict = HoardQuery(CR);

            string objStr = CalcObjStr(queryStrDict["object"]); //Parse Object String            
            string magStr = CalcMagicStr(queryStrDict["magic"]); //Parse Magic String

            hoardDict.Add("object", objStr);
            hoardDict.Add("magic", magStr);
            
            return hoardDict;
        }
        private Dictionary<string, string> HoardQuery(float CR)
        {
            //HoardQuery
            Random rand = new Random();
            int d100 = rand.Next(1, 100);
            string tblName = "tmpTableName";
            string queryString = "tmpStr";
            //[0:Gem/Art, 1:Magic]
            Dictionary<string, string> hoardQuery = new Dictionary<string, string>();
                        
            //Get hoard table name (hoard1, hoard2, ect)
            if (CR <= 4)
            {
                tblName = "HoardTbl1";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";

            }
            else if (CR <= 10)
            {
                tblName = "HoardTbl2";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";

            }
            else if (CR <= 16)
            {
                tblName = "HoardTbl3";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";

            }
            else if (CR >= 17)
            {
                tblName = "HoardTbl4";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";
            }

            hoardQuery = QueryHoardTbl(queryString); //key0=object, key1=magic, value=string for calculation
           
            return hoardQuery;
        }
        private Dictionary<string, string> QueryHoardTbl(string queryString)
        {
            //Console.WriteLine(queryString);
            Dictionary<string, string> queryDict = new Dictionary<string, string>(); //key=colName (gemOrObj, magicItem), value=string (ex. 2d6x100)
            DataTable dataTable = new DataTable();
            string connectionStr = "Data Source=..\\..\\sqlDatabase\\MasterDB.db;Version=3;New=False;Compress=False";
            SQLiteConnection connection = new SQLiteConnection(connectionStr); // string connString = @"your connection string here";
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command); // create data adapter
            dataAdapter.Fill(dataTable); // query db and return the result to your datatable
            dataAdapter.Dispose();
            connection.Close();

            var hoardArray = dataTable.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();

            queryDict.Add("object", hoardArray[1]);
            queryDict.Add("magic", hoardArray[2]);
            
            return queryDict;
        }
        private string CalcObjStr(string objStr)
        {
            if(objStr == "null")
            {
                return null;
            }
            string tmpStr = objStr;
            char[] delimiterChars = { 'd', ' ' };
            string[] parsedArray = tmpStr.Split(delimiterChars);
            string tblName = parsedArray[2];
            string queryStr = "SELECT * FROM " + tblName + " ORDER BY RANDOM() LIMIT 1;";
            string lootDesc = QueryObjTbl(queryStr);

            return lootDesc;
        }
        private string CalcMagicStr(string magStr)
        {
            if(magStr == "null")
            {
                return null;
            }            
            
            char[] delimiterChars = { 'd', ' ' };
            string[] parsedArray = magStr.Split(delimiterChars);
            int multiplier = Int32.Parse(parsedArray[1]);
            string tblLetter = parsedArray[2];
            
            string tblName = "MagicItems" + tblLetter + "Tbl";
            string queryStr = "SELECT * FROM " + tblName + " ORDER BY RANDOM() LIMIT 1;";
            string queryResultStr = QueryMagTbl(queryStr);
            
            Random rand = new Random();
            int itemAmt = rand.Next(1, multiplier);
            string lootStr = itemAmt.ToString() + " " + queryResultStr;
            return lootStr;
        }
        private string QueryObjTbl(string queryString)
        {
            //Console.WriteLine(queryString);
            Dictionary<string, string> queryDict = new Dictionary<string, string>(); //key=colName (CP, SP, etc), value=string (ex. 2d6x100)
            DataTable dataTable = new DataTable();
            string connectionStr = "Data Source=..\\..\\sqlDatabase\\MasterDB.db;Version=3;New=False;Compress=False";
            SQLiteConnection connection = new SQLiteConnection(connectionStr); // string connString = @"your connection string here";
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command); // create data adapter
            dataAdapter.Fill(dataTable); // query db and return the result to your datatable
            dataAdapter.Dispose();
            connection.Close();

            var lootArray = dataTable.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
            string objStr = lootArray[1];
            
            return objStr;
        }
        private string QueryMagTbl(string queryString)
        {
            //Console.WriteLine(queryString);
            Dictionary<string, string> queryDict = new Dictionary<string, string>(); //key=colName (CP, SP, etc), value=string (ex. 2d6x100)
            DataTable dataTable = new DataTable();
            string connectionStr = "Data Source=..\\..\\sqlDatabase\\MasterDB.db;Version=3;New=False;Compress=False";
            SQLiteConnection connection = new SQLiteConnection(connectionStr); // string connString = @"your connection string here";
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command); // create data adapter
            dataAdapter.Fill(dataTable); // query db and return the result to your datatable
            dataAdapter.Dispose();
            connection.Close();

            var magArray = dataTable.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
            string magStr = magArray[3];

            return magStr;
        }
        private Dictionary<string, int> CoinQuery(float CR, bool hoardTbl)
        {
            Random rand = new Random();
            int d100 = rand.Next(1, 100);
            string tblName = "tmpTableName";
            string queryString = "tmpStr";
            Dictionary<string, string> coinQuery = new Dictionary<string, string>();
            Dictionary<string, int> coinDict = new Dictionary<string, int>();

            //Get table name (hoard, coin1, coin2, etc)
            if (hoardTbl == true)
            {
                tblName = "CoinHoardTbl";
                queryString = "SELECT * FROM " + tblName + " WHERE CR == " + CR + ";";

            }
            else if (CR <= 4)
            {
                tblName = "CoinTbl1";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";

            }
            else if (CR <= 10)
            {
                tblName = "CoinTbl2";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";

            }
            else if (CR <= 16)
            {
                tblName = "CoinTbl3";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";

            }
            else if (CR >= 17)
            {
                tblName = "CoinTbl4";
                queryString = "SELECT * FROM " + tblName + " WHERE d100 == " + d100 + ";";

            }

            coinQuery = QueryCoinTbl(queryString); //key="cp", value=string for calculation

            //convert query dict to calculated totals
            foreach (KeyValuePair<string, string> item in coinQuery)
            {
                int[] parseArr = new int[2];
                int rollNum = 0;
                int multiplier = 0;
                int coinTotal = 0;
                if (item.Value != "")
                {
                    parseArr = CoinStringParser(item.Value);
                    rollNum = parseArr[0];
                    multiplier = parseArr[2];
                }

                for (int i = 0; i <= rollNum; i++) //calculate string
                {
                    coinTotal += rand.Next(1, 6);
                }
                coinTotal *= multiplier;
                coinDict.Add(item.Key, coinTotal);
            }

            return coinDict; //Returns array of totals for each coin type
        }
        private Dictionary<string, string> QueryCoinTbl(string queryString)
        {
            //Console.WriteLine(queryString);
            Dictionary<string, string> queryDict = new Dictionary<string, string>(); //key=colName (CP, SP, etc), value=string (ex. 2d6x100)
            DataTable dataTable = new DataTable();
            string connectionStr = "Data Source=..\\..\\sqlDatabase\\MasterDB.db;Version=3;New=False;Compress=False";
            SQLiteConnection connection = new SQLiteConnection(connectionStr); // string connString = @"your connection string here";
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command); // create data adapter
            dataAdapter.Fill(dataTable); // query db and return the result to your datatable
            dataAdapter.Dispose();
            connection.Close();

            var coinArray = dataTable.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();

            queryDict.Add("CP", coinArray[1]);
            queryDict.Add("SP", coinArray[2]);
            queryDict.Add("EP", coinArray[3]);
            queryDict.Add("GP", coinArray[4]);
            queryDict.Add("PP", coinArray[5]);

            return queryDict;
        }
        private int[] CoinStringParser(string tmpStr)
        {
            int[] parsedArray = new int[3];
            int i = 0;
            char[] delimiterChars = { 'd', 'x' };
            string[] words = tmpStr.Split(delimiterChars);

            foreach (string word in words)
            {
                parsedArray[i] = Int32.Parse(word);
                i++;
            }

            return parsedArray;
        }
        private Dictionary<string, float> QueryMonsterTbl(string filePath, string queryString)
        {
            Dictionary<string, float> queryDict = new Dictionary<string, float>();

            string temp = "Data Source=";
            temp += filePath;
            temp += ";Version=3;New=False;Compress=False";

            SQLiteConnection connection = new SQLiteConnection(temp); // Create a new database connection:
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                queryDict.Add(reader.GetString(0), reader.GetFloat(1));
            }
            reader.Close();
            connection.Close();

            return queryDict;
        }
        private List<KeyValuePair<string, float>> SelectMonsters(Dictionary<string, float> queryList, int monsterNum, int monstTotalXp)
        {
            int listSize = queryList.Count();
            Random rand = new Random();
            List<KeyValuePair<string, float>> list = new List<KeyValuePair<string, float>>();

            for (int i=0; i<monsterNum; i++)
            {
                int index = rand.Next(0, listSize);
                list.Add(queryList.ElementAt(index));
            }


            /*
            Random rand = new Random();
            int size = queryList.Count();
            int monstCnt = 0;
            int remainingXp = monstTotalXp;
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

            while (monstCnt < monsterNum)
            {
                int index = rand.Next(0, size);
                if (queryList.ElementAt(index).Value <= remainingXp)
                {
                    list.Add(new KeyValuePair<string, int>(queryList.ElementAt(index).Key, queryList.ElementAt(index).Value));
                    monstCnt++;
                    remainingXp -= queryList.ElementAt(index).Value;
                }
                else if (remainingXp < 10)
                {
                    list.Add(new KeyValuePair<string, int>(queryList.ElementAt(index).Key, queryList.ElementAt(index).Value));
                    monstCnt++;
                }
            }
            */

            return list;
        }
        private Dictionary<string, string> CombineMonsters(List<KeyValuePair<string, float>> monstersList)
        {
            List<KeyValuePair<string, float>> list = monstersList.ToList();
            Dictionary<string, string> generatedEncounter = new Dictionary<string, string>();

            var q = from x in list
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            foreach (var x in q)
            {
                //Console.WriteLine("Value: " + x.Value + " Count: " + x.Count);
                generatedEncounter.Add(x.Value.Key, x.Count.ToString());
            }

            return generatedEncounter;
        }
        private int calcTotalXP(string partySize, string partyLevel, string difficulty)
        {
            int size = Int32.Parse(partySize);
            int level = Int32.Parse(partyLevel);
            int partyXP = 0;

            string queryString = "SELECT " + difficulty + " FROM CharLevelXP WHERE CharLevel = " + level + ";";
            DataSet queryResult = QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", queryString);
            Dictionary<string, string> queryDic = Query(queryResult); //returned: key = field value, value = colName
            int levelXP = Int32.Parse(queryDic.First().Key);
            partyXP = levelXP * size;

            return partyXP;
        }
        private int getMonsterNum()
        {
            Random rand = new Random();
            int numMonstersSeed = rand.Next(1, 100);
            int numMonsters = 0;

            if (numMonstersSeed < 30) { numMonsters = 1; }
            else if (numMonstersSeed < 55) { numMonsters = 2; }
            else if (numMonstersSeed < 75) { numMonsters = rand.Next(3, 6); }
            else if (numMonstersSeed < 90) { numMonsters = rand.Next(7, 10); }
            else if (numMonstersSeed < 98) { numMonsters = rand.Next(11, 14); }
            else { numMonsters = rand.Next(15, 20); }

            return numMonsters;
        }
        private double getMonsterMultiplier(int monsterNum)
        {
            double multiplier = 0;

            if (monsterNum == 1) { multiplier = 1; }
            else if (monsterNum == 2) { multiplier = 1.5; }
            else if (monsterNum <= 6) { multiplier = 2; }
            else if (monsterNum <= 10) { multiplier = 2.5; }
            else if (monsterNum <= 14) { multiplier = 3; }
            else { multiplier = 4; }

            return multiplier;
        }
        private float getCR(int xp)
        {
            //highest xp in table = 155000
            if (xp > 155000) { return 30; }
            
            string queryString = "SELECT cr FROM CRtoXPTbl WHERE xp > " + xp + " ORDER BY xp ASC LIMIT 1;";
            float cr;
            string temp = "Data Source=..\\..\\sqlDatabase\\MasterDB.db;Version=3;New=False;Compress=False";

            SQLiteConnection connection = new SQLiteConnection(temp); // Create a new database connection:
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            cr = reader.GetFloat(0);
            reader.Close();
            connection.Close();

            return cr;
        }

        #endregion
    }
}
