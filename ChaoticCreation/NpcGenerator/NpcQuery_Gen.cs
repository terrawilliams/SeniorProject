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
        /// <summary>
        /// Sets up initial grammar data to be used in NPC descriptions
        /// </summary>
        public NpcQuery_Gen()
        {
            occupationFunction = new Dictionary<string, Func<Dictionary<string, string>>>
            {
                
            };

            genderSubject = new Dictionary<string, string>();
            genderObject = new Dictionary<string, string>();
            genderNoun = new Dictionary<string, string>();

            //English is hard
            genderSubject["Male"] = "He";
            genderSubject["Female"] = "She";

            genderObject["Male"] = "His";
            genderObject["Female"] = "Her";

            genderNoun["Male"] = "Man";
            genderNoun["Female"] = "Woman";

        }
        #endregion

        #region Members        
        Dictionary<string, Func<Dictionary<string, string>>> occupationFunction;
        string race;
        string raceAdjective;
        string gender;
        string occupation;
        Dictionary<string, string> genderSubject;
        Dictionary<string, string> genderObject;
        Dictionary<string, string> genderNoun;
        string vowel = "aeiouAEIOU";

        #endregion

        #region Methods
        /// <summary>
        /// Creates an NPC name and description based on the user specifications
        /// </summary>
        /// <param name="userSpecifiedData"></param>
        /// <returns>Dictionary containing the name and description of the
        /// generated NPC</returns>
        public Dictionary<string, string> NpcQuery(Dictionary<string, string> userSpecifiedData)
        {
            
            race = userSpecifiedData["race"];
            gender = userSpecifiedData["gender"];
            occupation = userSpecifiedData["occupation"];

            //retrieve the adjective for the selected race
            raceAdjective = Query(QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", "SELECT RaceAdj FROM NPCRaces WHERE Race = \"" + race + "\";")).ElementAt(0).Key;

            Dictionary<string, string> generatedNpc = new Dictionary<string, string>();
            generatedNpc["description"] = Generic();

            Dictionary<string, string> name = NpcName();

            generatedNpc["name"] = name["first"] + " " + name["last"];

            return generatedNpc;
        }
        /// <summary>
        /// Randomly generates an NPC name given a requested gender
        /// </summary>
        /// <returns>A dictionary containg the first and last name created</returns>
        private Dictionary<string, string> NpcName()
        {
            Dictionary<string, string> tableList = new Dictionary<string, string>
            {
                { "last", "LastNamesTbl"},
            };

            if(gender == "Female")
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
        /// <summary>
        /// Creates a generic physical description of an NPC
        /// </summary>
        /// <returns>Generic physical description of an NPC</returns>
        private string Generic()
        {
            Random genNum = new Random();

            Dictionary<string, string> tableList = new Dictionary<string, string>{
                {"body", "BodyTbl"},
                {"height", "HeightTbl" },
                {"chin", "ChinJawTbl" },
                {"ears", "EarsTbl" },
                {"hair", "HairTbl" },
                {"hands", "HandsTbl" },
                {"mouth", "MouthTbl" },
                {"nose", "NoseTbl" },
                {"tattoo", "TattooTbl" },
                {"bodypart", "BodyPartTbl" },
                { "placement", "PrepositionsTbl" },
                {"scar", "ScarTbl" }
            };

            Dictionary<string, string> values = runQueries(tableList);

            string description = "A";
            //height description
            if(genNum.Next(0,4) == 0)
            {
                //Get the placement of the height phrase in the sentence
                string heightPlacement = Query(QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", "SELECT grammarType FROM HeightTbl WHERE d6Height = \"" + values["height"] + "\";")).ElementAt(0).Key;

                //Height description at the end of the sentence
                if(heightPlacement == "end")
                {
                    description += isVowel(raceAdjective)
                        + raceAdjective.ToLower() + " "
                        + genderNoun[gender].ToLower() + " of "
                        + values["height"].ToLower()
                        + " approaches you.";
                }
                //Height description in the middle of the sentence
                else
                {
                    description += isVowel(values["height"])
                        + values["height"].ToLower() + " "
                        + raceAdjective.ToLower() + " "
                        + genderNoun[gender].ToLower()
                        + " approaches you.";
                }

            }
            //body description
            else
            {
                description += isVowel(values["body"])
                        + values["body"].ToLower() + " "
                        + raceAdjective.ToLower() + " "
                        + genderNoun[gender].ToLower()
                        + " approaches you.";
            }
            //Randomly select an additional feature that the NPC has
            List<int> randomFeatures = generateDistinctNumbers(2, 7);
            string feature1 = getFeature(randomFeatures[0], values);
            string feature2 = getFeature(randomFeatures[1], values);

            description += " "
                + genderSubject[gender] + " "
                + "has " + feature1.ToLower()
                + " and " + feature2.ToLower()
                + ".";

            //25% chance of additional feature
            if(genNum.Next(0, 4) == 0)
            {
                description += " You notice " + genderSubject[gender].ToLower() + " has ";

                //50% chance scar
                if(genNum.Next(0, 2) == 0)
                {
                    description += values["scar"].ToLower();
                }
                //50% chance tattoo
                else
                {
                    description += values["tattoo"].ToLower() + " tattoo";
                }

                description += " "
                        + values["placement"].ToLower() + " "
                        + genderObject[gender].ToLower() + " "
                        + values["bodypart"].ToLower() + ".";
            }




            return description;
        }
        /// <summary>
        /// Given a dictionary of tables, runQueries selects one random record from
        /// </summary>
        /// <param name="tables"></param>
        /// <returns>Dictionary containing query results</returns>
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
        /// <summary>
        /// Checks if a word or phrase begins with a vowel
        /// </summary>
        /// <param name="word"></param>
        /// <returns>An empty string if the word does not start with a vowel
        /// and a "n " if the word does start with a vowel</returns>
        string isVowel(string word)
        {
            if (vowel.IndexOf(word[0]) >= 0)
            {
                return "n ";
            }
            return " ";
        }
        /// <summary>
        /// Helper function to assist in choosing random physical features for an NPC
        /// </summary>
        /// <param name="featureChoice"></param>
        /// <param name="features"></param>
        /// <returns>String containg the randomly chosen NPC feature</returns>
        string getFeature(int featureChoice, Dictionary<string, string> features)
        {
            string feature = "";

            switch (featureChoice)
            {
                case 1:
                    feature = features["chin"];
                    break;
                case 2:
                    feature = features["ears"];
                    break;
                case 3:
                    feature = features["hands"];
                    break;
                case 4:
                    feature = features["mouth"];
                    break;
                case 5:
                    feature = features["nose"];
                    break;
                case 6:
                    feature = features["hair"];
                    break;
            }

            return feature;
        }
        /// <summary>
        /// Helper function to generate a requested number of distinct random integers
        /// </summary>
        /// <param name="number_of_numbers"></param>
        /// <param name="range"></param>
        /// <returns>List of distinct random integers</returns>
        List<int> generateDistinctNumbers(int number_of_numbers, int range)
        {
            Random genNum = new Random();

            List<int> numbers = new List<int>();
            numbers.Add(genNum.Next(1, range));
            int number;

            for(int i = 0; i < number_of_numbers; i++)
            {
                do
                {
                    number = genNum.Next(1, range);
                } while (numbers.Contains(number));

                numbers.Add(number);
            }

            return numbers;
        }
        #endregion
    }
 
}
