using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ChaoticCreation.LocationGenerator
{
    class LocationQuery_Gen : GeneralGenerator
    {
        #region Constructors
        public LocationQuery_Gen()
        {
            //ideally this would be dynamic and pull from the database but Imma have to revisit it later
            locationFunction = new Dictionary<string, Func<string, Dictionary<string, string>>>
            {
                {"Castle", Castle},
                {"Cave", Cave},
                {"Desert", Desert},
                {"Forest", Forest},
                {"Jungle", Jungle},
                {"Mine", Mine},
                {"Mountain", Mountain},
                {"Tavern", Tavern},
                {"Temple", Temple}
            };

        }
        #endregion

        #region Members        
        Dictionary<string, Func<string, Dictionary<string, string>>> locationFunction;
        #endregion

        #region Methods

        public Dictionary<string, string> LocationQuery(List<string> userSpecifiedData)
        {
            //userSpecifiedData[0] = type, [1] = size
            string locationType = userSpecifiedData[0];
            string locationSize = userSpecifiedData[1];

            //Call the appropriate location method
            Dictionary<string, string> generatedLocation = locationFunction[locationType].Invoke(locationSize);

            generatedLocation["description"] = "Type: " + locationType + "\n" + generatedLocation["description"];

            return generatedLocation;
        }

        private Dictionary<string, string> Castle(string size)
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
        private Dictionary<string, string> Cave(string size)
        {

            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"name", "CaveNamesTbl"},
                {"entrance", "CaveEntranceTbl" },
                {"inhabitants", "CaveInhabitantsTbl" },
                {"ecology", "CaveEcologyTbl" },
                {"feature", "CaveFeatureTbl" },
                {"wall", "CaveWallTbl" },
                {"ceiling","CaveCeilingTbl" },
                {"hazard","CaveHazardTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string caveName = values["name"];

            string description =
                "The cave mouth is " +
                values["entrance"].ToLower() +
                ". The cave is inhabited by " +
                values["inhabitants"].ToLower() +
                ". The creature is " +
                values["ecology"].ToLower() +
                " You notice " +
                values["feature"].ToLower() +
                " The cave wall is " +
                values["wall"].ToLower() +
                " The ceiling is " +
                values["ceiling"].ToLower() +
                " You find " +
                values["hazard"].ToLower();

            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", caveName},
                {"description", description}
            };

            return results;
        }
        private Dictionary<string, string> Desert(string size)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"name", "DesertNamesTbl"},
                {"location", "DesertLocationTbl" },
                {"landmark", "DesertLandmarkTbl" },
                {"feature", "DesertFeatureTbl" },
                {"landscape", "DesertLandscapeTbl" },
                {"ground", "DesertGroundTbl" },
                {"hazard","DesertHazardsTbl" },
                {"climate","DesertClimateTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string desertName = values["name"];

            string description =
                 values["name"] +
                 " is " +
                 values["landscape"].ToLower() +
                 " The earth beneath your feet is " +
                 values["ground"].ToLower() +
                 ". The rains come to this desert " +
                 values["climate"].ToLower() +
                 ". You find " +
                 values["location"].ToLower() +
                 " and " +
                 values["landmark"].ToLower() +
                 " You notice " +
                 values["feature"].ToLower() +
                 " when you run into " +
                 values["hazard"].ToLower() + ".";

            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", desertName},
                {"description", description}
            };

            return results;
        }
        private Dictionary<string, string> Forest(string size)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"name", "ForestNamesTbl"},
                {"trees", "ForestTreesTbl" },
                {"location", "ForestLocationTbl" },
                {"landmark", "ForestLandmarkTbl" },
                {"feature", "ForestFeatureTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string forestName = values["name"];

            string description =
                "In " +
                 values["name"] +
                 " many of the trees are " +
                 values["trees"].ToLower() +
                 ". You find " +
                values["location"].ToLower() +
                " and " +
                values["landmark"].ToLower() +
                ". You notice " +
                values["feature"].ToLower() + ".";


            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", forestName},
                {"description", description}
            };

            return results;

        }
        private Dictionary<string, string> Jungle(string size)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"name", "JungleNamesTbl"},
                {"feature", "JungleFeatureTbl" },
                {"location", "JungleLocationTbl" },
                {"landmark", "JungleLandmarkTbl" },
                {"lair", "JungleLairTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string jungleName = values["name"];

            string description =
                "You notice " +
                values["feature"].ToLower() +
                ". You find " +
                values["location"].ToLower() +
                " and " +
                values["landmark"].ToLower() +
                ". You eventually come across " +
                values["lair"].ToLower() + ".";


            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", jungleName},
                {"description", description}
            };

            return results;
        }
        private Dictionary<string, string> Mine(string size)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"name", "MineNamesTbl"},
                {"feature", "MineFeaturesTbl" },
                {"ecology", "MineEcologyTbl" },
                {"entrance", "MineEntranceTbl" },
                {"hazard", "MineHazardsTbl" },
                {"inhabitants", "MineInhabitantsTbl" },
                {"type", "MineTypeTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string mineName = values["name"];

            string description =
                values["name"] +
                " is a source of " +
                values["type"].ToLower() +
                " The mine's entrance is " +
                values["entrance"].ToLower() +
                " You notice " +
                values["feature"].ToLower() +
                " You find " +
                values["hazard"].ToLower() +
                " If you dig deep you'll find " +
                values["inhabitants"].ToLower() +
                " The creature is " +
                values["ecology"].ToLower();



            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", mineName},
                {"description", description}
            };

            return results;
        }
        private Dictionary<string, string> Mountain(string size)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"name", "MountainNamesTbl"},
                {"feature", "MountainFeatureTbl" },
                {"hazard", "MountainHazardTbl" },
                {"lair", "MountainLairTbl" },
                {"landmark", "MountainLandmarkTbl" },
                {"location", "MountainLocationTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string mountainName = values["name"];

            string description =
                "On " +
                values["name"] +
                " you come across " +
                values["landmark"].ToLower() +
                " and " +
                values["lair"].ToLower() + "." +
                " You notice " +
                values["feature"].ToLower() +
                " when you run into " +
                values["hazard"].ToLower() + "." +
                " You notice " +
                values["location"].ToLower() +
                " in the distance.";

            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", mountainName},
                {"description", description}
            };

            return results;
        }

        private Dictionary<string, string> Tavern(string size)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"entertainment", "TavernEntertainmentTbl" },
                {"patrons", "TavernPatronsTbl" },
                {"drink", "TavernDrinksTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string tavernName = createTavernName();

            string description =
                values["patrons"] +
                " " + values["entertainment"] +
                "\n\nSpecialty Drink: " +
                values["drink"];

            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", tavernName},
                {"description", description}
            };

            return results;
        }

        private string createTavernName()
        {
            string table = "TavernNameTbl";
            List<string> queries = new List<string>();

            queries.Add("SELECT Verb FROM " + table + " ORDER BY RANDOM() LIMIT 1;");
            queries.Add("SELECT Adjective FROM " + table + " ORDER BY RANDOM() LIMIT 1;");
            queries.Add("SELECT Noun1 FROM " + table + " ORDER BY RANDOM() LIMIT 1;");
            queries.Add("SELECT Noun2 FROM " + table + " ORDER BY RANDOM() LIMIT 1;");

            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();

            foreach (string queryString in queries)
            {
                values.Add(Query(QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", queryString)));
            }

            string verb = values[0].ElementAt(0).Key;
            string adjective = values[1].ElementAt(0).Key;
            string noun1 = values[2].ElementAt(0).Key;
            string noun2 = values[3].ElementAt(0).Key;

            Random rando = new Random();

            string name = "";

            switch (rando.Next(1, 4))
            {
                case 1:
                    name = "The " + adjective + " " + noun1;
                    break;
                case 2:
                    if (rando.Next(0, 1) != 0)
                    {
                        name = "The " + noun1 + " and " + noun2;
                    }
                    else
                    {
                        name = "The " + noun1 + " and the " + noun2;
                    }
                    break;
                case 3:
                    name = "The " + noun1 + "'s " + noun2;
                    break;
                case 4:
                    if (rando.Next(0, 1) != 0)
                    {
                        name = "The " + verb + " " + noun1;
                    }
                    else
                    {
                        name = "The " + verb + " " + noun2;
                    }
                    break;
            }


            return name;
        }

        private Dictionary<string, string> Temple(string size)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"name", "TempleNamesTbl"},
                {"feature", "MountainFeatureTbl" },
                {"ceiling", "TempleCeilingsTbl" },
                {"entrance", "TempleEntranceTbl" },
                {"history", "TempleHistoryTbl" },
                {"room", "TempleRoomsTbl" },
                {"wall", "TempleWallsTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string templeName = values["name"];

            string description =
                "As you approach you notice the temple is guarded by " +
                values["entrance"].ToLower() +
                " " + values["name"] +
                " is known for its " +
                values["history"].ToLower() +
                " The walls are " +
                values["wall"].ToLower() +
                " and you notice " +
                values["feature"].ToLower() +
                ". The purpose of this room is " +
                values["room"].ToLower() +
                " and many of the temples ceilings are " +
                values["ceiling"].ToLower();

            Dictionary<string, string> results = new Dictionary<string, string>
            {
                {"name", templeName},
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