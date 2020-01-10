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
        Dictionary<string, Func<string, Dictionary<string, string>>> encounterFunction;
        #endregion

        #region Methods
        public Dictionary<string, string> EncounterQuery(List<string> userSpecifiedData)
        {
            //userSpecifiedData[0] = size, [1] = level, etc
            string partySize = userSpecifiedData[0];
            string partyLevel = userSpecifiedData[1];
            string terrain = userSpecifiedData[2];
            string difficulty = userSpecifiedData[3];

            //Call the appropriate encounter method
            Dictionary<string, string> generatedEncounter = encounterFunction[encounterType].Invoke(encounterSize);

            generatedEncounter["description"] = "Type: " + encounterType + "\n" + generatedEncounter["description"];

            return generatedEncounter;
        }
        private Dictionary<string, string> runQueries(Dictionary<string,string> tables)
        {
            List<string> queries = new List<string>();
            string query;

            //Create the queries for the given list of tables
            foreach(string table in tables.Values)
            {
                query = QueryEdit(table);
                queries.Add(query);
            }

            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();

            //Run all the queries (All encounter queries are generic)
            foreach (string queryString in queries)
            {
                values.Add(Query(QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", queryString)));
            }

            Dictionary<string, string> results = new Dictionary<string, string>();

            //Create dictionary for easy access to returned values in individual functions
            for(int i = 0; i < values.Count; i++)
            {
                results[tables.ElementAt(i).Key] = values[i].ElementAt(0).Key;            
            }

            return results;
        }
        #endregion
    }
}
