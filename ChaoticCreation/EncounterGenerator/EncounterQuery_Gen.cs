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
        public Dictionary<string, string> EncounterQuery(List<string> userSpecifiedData)
        {
            //userSpecifiedData[0] = size, [1] = level, etc
            string partySize = userSpecifiedData[0];
            string partyLevel = userSpecifiedData[1];
            string terrain = userSpecifiedData[2];
            string difficulty = userSpecifiedData[3];

            //Encounter Calculation
            int partyXP = calcTotalXP(partySize, partyLevel, difficulty); //Returns party XP Threshold
            int monsterNum = getMonsterNum(); //Returns random number of monsters
            double monsterMultiplier = getMonsterMultiplier(monsterNum); //Returns multiplier based on number of monsters
            int monsterTotalXp = Convert.ToInt32(partyXP / monsterMultiplier); //Total XP for all monsters

            Console.WriteLine("VALUES");
            Console.WriteLine("monsterNum = " + monsterNum);
            Console.WriteLine("terrain = " + terrain);
            Console.WriteLine("monsterTotalXp = " + monsterTotalXp);

            if (monsterTotalXp < 10) { monsterTotalXp = 10; }
            string queryString = "SELECT monsterName, xp FROM monsterTbl WHERE xp <= " + monsterTotalXp + 
                " AND environment LIKE '%" + terrain + "%' ORDER BY xp;";
            Dictionary<string, int> queryResult = QueryMonsterTbl("..\\..\\sqlDatabase\\MasterDB.db", queryString);
            if(queryResult.Count() == 0)
            {
                queryString = "SELECT monsterName, xp FROM monsterTbl WHERE xp<=" + monsterTotalXp + 
                    " AND environment IS Null ORDER BY xp;";
                queryResult = QueryMonsterTbl("..\\..\\sqlDatabase\\MasterDB.db", queryString);
            }
            
            List<KeyValuePair<string, int>> tempDictionary = SelectMonsters(queryResult, monsterNum, monsterTotalXp);
            Dictionary<string, string> generatedEncounter = CombineMonsters(tempDictionary);
            Console.WriteLine("Generated Encounter");
            foreach (KeyValuePair<string, string> item in generatedEncounter)
            {
                Console.WriteLine("Key: " + item.Key + " Value: " + item.Value);
            }

            float cr = getCR(monsterTotalXp);
            //Console.WriteLine("CR: " + cr);
            
            Dictionary<string, int> lootDict = new Dictionary<string, int>();
            lootDict = lootCalc(cr, monsterNum);

            return generatedEncounter;
        }
        private Dictionary<string, int> QueryMonsterTbl(string filePath, string queryString)
        {
            Dictionary<string, int> queryDict = new Dictionary<string, int>();
            
            string temp = "Data Source=";
            temp += filePath;
            temp += ";Version=3;New=False;Compress=False";

            SQLiteConnection connection = new SQLiteConnection(temp); // Create a new database connection:
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                queryDict.Add(reader.GetString(0), reader.GetInt32(1));
            }
            connection.Close();

            return queryDict; 
        }
        private List<KeyValuePair<string, int>> SelectMonsters(Dictionary<string, int> queryList, int monsterNum, int monstTotalXp)
        {
            Random rand = new Random();
            int size = queryList.Count();
            int monstCnt = 0;
            int remainingXp = monstTotalXp;
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

            while(monstCnt < monsterNum)
            {
                int index = rand.Next(0, size);
                if(queryList.ElementAt(index).Value <= remainingXp)
                {
                    list.Add(new KeyValuePair<string, int>(queryList.ElementAt(index).Key, queryList.ElementAt(index).Value));
                    monstCnt++;
                    remainingXp -= queryList.ElementAt(index).Value;
                }
                else if(remainingXp < 10)
                {
                    list.Add(new KeyValuePair<string, int>(queryList.ElementAt(index).Key, queryList.ElementAt(index).Value));
                    monstCnt++;
                }
            }
            return list;
        }
        private Dictionary<string, string> CombineMonsters(List<KeyValuePair<string, int>> monstersList)
        {
            List<KeyValuePair<string, int>> list = monstersList.ToList();
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

            if(numMonstersSeed < 30) {numMonsters = 1;}
            else if (numMonstersSeed < 55) {numMonsters = 2;}
            else if (numMonstersSeed < 75) {numMonsters = rand.Next(3,6);}
            else if (numMonstersSeed < 90) {numMonsters = rand.Next(7,10);}
            else if (numMonstersSeed < 98) {numMonsters = rand.Next(11,14);}
            else {numMonsters = rand.Next(15,20);}

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
            string queryString = "SELECT cr FROM CRtoXPTbl WHERE xp > " + xp + " ORDER BY xp ASC LIMIT 1;";
            float cr;
            string temp = "Data Source=..\\..\\sqlDatabase\\MasterDB.db;Version=3;New=False;Compress=False";

            SQLiteConnection connection = new SQLiteConnection(temp); // Create a new database connection:
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            reader.Read();
            cr = reader.GetFloat(0);
            connection.Close();
            
            return cr;
        }
        private Dictionary<string, int> lootCalc(float CR, int monsterNum)
        {
            string lootStr;
            Dictionary<string, int> coinDict = new Dictionary<string, int>();

            if(CR < 1)
            {
                CR = 0;
            }
            
            if (monsterNum == 1)
            {
                coinDict = CoinQuery(CR, false);
            }
            else if(monsterNum > 1)
            {
                Console.WriteLine("CR = " + CR);
                coinDict = CoinQuery(CR, true);
                //HoardQuery(CR);
            }

            foreach (KeyValuePair<string, int> item in coinDict)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }

            return coinDict;
        }

        private Dictionary<string, int> CoinQuery(float CR, bool hoardTbl)
        {
            Random rand = new Random();
            int d100 = rand.Next(1, 100);
            string tblName = "tmpTableName";
            string queryString = "tmpStr";
            Dictionary<string, string> coinQuery = new Dictionary<string, string>();
            Dictionary<string, int> coinDict = new Dictionary<string, int>();

            //Get table name
            if(hoardTbl == true)
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
            
            foreach (KeyValuePair<string, string> item in coinQuery) //convert query dict to calculated totals
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

                for(int i=0; i<=rollNum; i++) //calculate string
                {
                    coinTotal += rand.Next(1, 6);
                }
                coinTotal *= multiplier;
                coinDict.Add(item.Key, coinTotal);
            }

            return coinDict;
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
        private void HoardQuery(float CR)
        {
            //CoinHoardQuery
            //HoardQuery
        }

        private Dictionary<string, string> QueryCoinTbl(string queryString)
        {
            Console.WriteLine(queryString);
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

        #endregion
    }
}
