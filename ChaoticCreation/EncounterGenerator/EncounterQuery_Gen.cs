using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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
            
            
            int eachMonsterXp = monsterTotalXp / monsterNum; //Gives XP for individual monsters
            double monsterCR = getCR(eachMonsterXp); //Returns the CR value for the given XP value
            
            Console.WriteLine("VALUES");
            Console.WriteLine("monsterNum = " + monsterNum);
            Console.WriteLine("TEST: monsterCR = " + monsterCR);
            Console.WriteLine("terrain = " + terrain);
            //Console.WriteLine("monsterMultiplier = " +  monsterMultiplier);
            Console.WriteLine("monsterTotalXp = " + monsterTotalXp);
            //Console.WriteLine("TEST: eachMonsterXp = " + eachMonsterXp);
            //Console.WriteLine("partyXP = " + partyXP);

            string queryString = "SELECT monsterName FROM monsterTbl WHERE CR = " + monsterCR + " AND environment LIKE '%" + terrain + "%';";
            DataSet queryResult = QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", queryString);
            Dictionary<string, string> queryDic = Query(queryResult); //returned: key = field value, value = colName
            List<string> monsterList = queryDic.Keys.ToList();
            Console.WriteLine("List Count = " + monsterList.Count);
            if(monsterList.Count == 0) //if the query doesn't return any results
            {
                Dictionary<string, string> noResults = new Dictionary<string, string>();
                noResults.Add("No Results", "No Results for Query");
                return noResults;
            }
            
            //foreach (string i in monsterList) { Console.WriteLine(i); }
            
            Dictionary<string, string> generatedEncounter = new Dictionary<string, string>();
                        
            var random = new Random();
            for (int i=0; i < monsterNum; i++)
            {
                int index = random.Next(monsterList.Count);
                string iToStr  = i.ToString();
                string monsterCount = "monst" + iToStr;
                generatedEncounter.Add(monsterCount, monsterList[index]);
            }

            return generatedEncounter;
        }

        public int calcTotalXP(string partySize, string partyLevel, string difficulty)
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
        public int getMonsterNum()
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
        private double getCR(int XpValue)
        {
            double CR = 0;

            if (XpValue <= 10) { CR = 0; }
            else if (XpValue <= 25) { CR = 0.125; }
            else if (XpValue <= 50) { CR = 0.25; }
            else if (XpValue <= 100) { CR = 0.5; }
            else if (XpValue <= 200) { CR = 1; }
            else if (XpValue <= 450) { CR = 2; }
            else if (XpValue <= 700) { CR = 3; }
            else if (XpValue <= 1100) { CR = 4; }
            else if (XpValue <= 1800) { CR = 5; }
            else if (XpValue <= 2300) { CR = 6; }
            else if (XpValue <= 2900) { CR = 7; }
            else if (XpValue <= 3900) { CR = 8; }
            else if (XpValue <= 5000) { CR = 9; }
            else if (XpValue <= 5900) { CR = 10; }
            else if (XpValue <= 7200) { CR = 11; }
            else if (XpValue <= 8400) { CR = 12; }
            else if (XpValue <= 10000) { CR = 13; }
            else if (XpValue <= 11500) { CR = 14; }
            else if (XpValue <= 13000) { CR = 15; }
            else if (XpValue <= 15000) { CR = 16; }
            else if (XpValue <= 18000) { CR = 17; }
            else if (XpValue <= 20000) { CR = 18; }
            else if (XpValue <= 22000) { CR = 19; }
            else if (XpValue <= 25000) { CR = 20; }
            else if (XpValue <= 33000) { CR = 21; }
            else if (XpValue <= 41000) { CR = 22; }
            else if (XpValue <= 50000) { CR = 23; }
            else if (XpValue <= 62000) { CR = 24; }
            else if (XpValue <= 75000) { CR = 25; }
            else if (XpValue <= 90000) { CR = 26; }
            else if (XpValue <= 105000) { CR = 27; }
            else if (XpValue <= 120000) { CR = 28; }
            else if (XpValue <= 135000) { CR = 29; }
            else { CR = 30; }

            return CR;
        }
        private IEnumerable<TKey> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TKey> values = Enumerable.ToList(dict.Keys);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }

        #endregion
    }
}
