using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//dataColumn
using System.Data;

namespace ChaoticCreation.NpcGenerator
{
    class NpcQuery_Gen : GeneralGenerator
    {
        #region Constructors
        public NpcQuery_Gen()
        {
            //ideally this would be dynamic and pull from the database but Imma have to revisit it later
            occupationFunction = new Dictionary<string, Func<Dictionary<string, string>>>
            {
                {"Actor", Actor }
            };

        }
        #endregion

        #region Members        
        Dictionary<string, Func<Dictionary<string, string>>> occupationFunction;
        string race;
        string gender;
        string occupation;
        #endregion

        #region Methods

        public Dictionary<string, string> NpcQuery(Dictionary<string, string> userSpecifiedData)
        {

            race = userSpecifiedData["race"];
            gender = userSpecifiedData["gender"];
            occupation = userSpecifiedData["occupation"];

            //Call the appropriate location method
            //Dictionary<string, string> generatedNpc = occupationFunction[occupation].Invoke();

            //generatedNpc["description"] = "Type: " + occupation + "\n" + generatedNpc["description"];

            Dictionary<string, string> generatedNpc = new Dictionary<string, string>();
            generatedNpc["description"] = "";

            Dictionary<string, string> name = NpcName();

            generatedNpc["name"] = name["first"] + " " + name["last"];

            return generatedNpc;
        }

        private Dictionary<string, string> NpcName()
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>
            {
                { "last", "LastNamesTbl"},
            };

            if (gender == "Female")
            {
                tableList["first"] = "FemaleNamesTbl";
            }
            else
            {
                tableList["first"] = "MaleNamesTbl";
            }

            Dictionary<string, string> name = runQueries(tableList);

            return name;
        }
        private Dictionary<string, string> Actor()
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
            {"name", "CastleNamesTbl"},
            {"location", "CastleLocationTbl" },
            {"inhabitants", "CastleInhabitantsTbl" },
            {"knownFor", "CastleKnownForTbl" },
            {"room", "CastleRoomsTbl" },
            {"feature", "CastleRoomFeatureTbl" }
        };

            Dictionary<string, string> values = runQueries(tableList);

            string castleName = values["name"];

            string description =
                "The castle sits " +
                values["location"].ToLower() +
                " Presently the castle is occupied by " +
                values["inhabitants"].ToLower() +
                " " + values["name"] +
                " is known for " +
                values["knownFor"].ToLower() +
                " This chamber is " +
                values["room"].ToLower() +
                " You notice " +
                values["feature"].ToLower();

            Dictionary<string, string> results = new Dictionary<string, string>
        {
            {"name", castleName},
            {"description", description}
        };

            return results;
        }

        private Dictionary<string, string> runQueries(Dictionary<string, string> tables)
        {
            List<string> queries = new List<string>();
            string query;

            //Create the queries for the given list of tables
            foreach (string table in tables.Values)
            {
                query = QueryEdit(table);
                queries.Add(query);
            }

            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();

            //Run all the queries (All location queries are generic)
            foreach (string queryString in queries)
            {
                values.Add(Query(QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", queryString)));
            }

            Dictionary<string, string> results = new Dictionary<string, string>();

            //Create dictionary for easy access to returned values in individual functions
            for (int i = 0; i < values.Count; i++)
            {
                results[tables.ElementAt(i).Key] = values[i].ElementAt(0).Key;
            }

            return results;
        }

        #endregion
    }

}
