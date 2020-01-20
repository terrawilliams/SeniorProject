using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

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
        
        #endregion
    }
}
