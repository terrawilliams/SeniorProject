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
        #region Members       

        #endregion

        #region Getters and Setters

        #endregion

        #region Methods
        //format the out dictionary into name, description
        private Dictionary<string, string> NpcFormat(List<Dictionary<string, string>> values, string name, string Race, bool gender)
        {
            string description =
                "Before you stands a ";
            description += Race;

            if (gender)
                description += " man, he is a";
            else
                description += " woman, she is a";


            // every "values[x]", where x is a number, corresponds to a specific table input from the "values" list of dictionaries which is queried from the sql db
            // this essentially grabs the specific unique key assosiated with a specific query. since we only grab a single value we grab the element at 0's key.  
            //the specific "x" values correspond to ordering in the tables section.... probably better to change this to an enum
            //generalDictOut is used and overwritten everytime we print out from values... its a little bit of bad coding practice, but I'm lazy and this is actually probably a little more efficient
            //8 is the occupation
            string generalDictOut = values[8].ElementAt(0).Key;

            //takes care of the case where the occupation starts with an a or not an a
            if ((generalDictOut.StartsWith("A")) || (generalDictOut.StartsWith("a")))
                description += "n ";
            else
                description += " ";

            description += generalDictOut;
            description += " their body is ";

            //0 is the body type
            generalDictOut = values[0].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += " that stands at a height that is ";

            //5 is the stature/height
            generalDictOut = values[5].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ". They also have ";

            //1 is the chin/jaw table
            generalDictOut = values[1].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ". Upon closer inspection, they have ";

            //2 is the ears
            generalDictOut = values[2].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ", they have ";

            //3 is the hair
            generalDictOut = values[3].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += " and they have ";

            //4 is the hands
            generalDictOut = values[4].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ". Their mouth has ";

            //6 is the mouth
            generalDictOut = values[6].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ", they also have ";

            //7 is the nose
            generalDictOut = values[7].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ".";

            //this is for tattoos
            if (values[9] != null)
            {
                description += " Lastly, they have ";

                generalDictOut = values[9].ElementAt(0).Key;
                description += generalDictOut.ToLower();

            }
            
            Dictionary<string, string> retValue = new Dictionary<string, string>();
            retValue.Add("name", name);
            retValue.Add("description", description);
            return retValue;
        }
 
        //loads in user specified data, queries db for it, and then puts it into correct formatting to be returned
        public Dictionary<string, string> NpcQuery(List<string> userSpecifiedData)
        {
            //userSpecifiedData[0] = race, [1] = gender, [2] = occupation
            var rand = new Random();

            //This order is the same as the order in database
            List<string> queries = new List<string>();
            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();

            //This is annoying sorry
            //Just add the name of the table being queried into the queryedit function and then add that to the list of queries that's going to be executed later
            string Body = QueryEdit("BodyTbl");
            queries.Add(Body);
            string ChinJaw = QueryEdit("ChinJawTbl");
            queries.Add(ChinJaw);
            string Ears = QueryEdit("EarsTbl");
            queries.Add(Ears);
            string Hair = QueryEdit("HairTbl");
            queries.Add(Hair);
            string Hands = QueryEdit("HandsTbl");
            queries.Add(Hands);
            string Height = QueryEdit("HeightTbl");
            queries.Add(Height);
            string Mouth = QueryEdit("MouthTbl");
            queries.Add(Mouth);
            string Nose = QueryEdit("NoseTbl");
            queries.Add(Nose);

            //this needs to be different because we are actually querying the database, unlike race or gender.
            //it also has two different queries depending on if the user selects the occupation or not
            string queryEditedOccupation = QueryEdit("Occupations");
            if (userSpecifiedData[2] != "Any")
            {
                queryEditedOccupation = QueryEdit("Occupations", GeneralQuery, "OccName", userSpecifiedData[2]);
            }
            queries.Add(queryEditedOccupation);

            string Tattoo = null;

            if (rand.Next(0,1) == 0)
            {
                Tattoo = QueryEdit("TattooTbl");
                queries.Add(Tattoo);
            }


            //names
            //This goes away at some point
            List<string> maleNames = new List<string>();
            maleNames.Add("Wynell Gussie");
            maleNames.Add("Pavlo Gresser");
            maleNames.Add("Chapman Valentine");
            maleNames.Add("Garris Edds");
            maleNames.Add("Bronson Keppler");
            maleNames.Add("Dell Gudrun");
            maleNames.Add("Kron Greenbolt");
            maleNames.Add("Jeremy Carlysle");

            List<string> femaleNames = new List<string>();
            femaleNames.Add("Kala Tien");
            femaleNames.Add("Genna Heine");
            femaleNames.Add("Ila Harriet");
            femaleNames.Add("Adrienne Shim");
            femaleNames.Add("Laree Kris");
            femaleNames.Add("Emeline Venita");
            femaleNames.Add("Selma Batwig");
            femaleNames.Add("Shu Kary");

            bool gender;
            string name;
            if (userSpecifiedData[1] == "Male")
            {
                name = maleNames[rand.Next(0, maleNames.Count)];
                gender = true;
            }
            else
            {
                name = femaleNames[rand.Next(0, femaleNames.Count)];
                gender = false;
            }
            //where we actually query everything
            foreach (string queryStrings in queries)
            {
                values.Add(Query(QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", queryStrings)));
            }
            //we then format it to be of type dictionary of name, description 
            Dictionary<string, string> returnValue = NpcFormat(values, name, userSpecifiedData[0], gender);
            //return the formatted data
            return returnValue;

        }
        #endregion

    }
}
