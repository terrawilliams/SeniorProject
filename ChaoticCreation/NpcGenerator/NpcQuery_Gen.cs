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
        //format the out dictionary
        private Dictionary<string, string> NpcFormat(List<Dictionary<string, string>> values, string name, string Race, bool gender)
        {
            string description =
                "Before you stands a ";
            description += Race;

            //descriptor query string not done
            if (gender)
                description += " man, he is a";
            else
                description += " woman, she is a";

            string generalDictOut = values[8].ElementAt(0).Key;

            if ((generalDictOut.StartsWith("A")) || (generalDictOut.StartsWith("a")))
                description += "n ";
            else
                description += " ";
            description += generalDictOut;
            description += " their body is ";

            generalDictOut = values[0].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += " that stands at a height that is ";

            generalDictOut = values[5].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ". They also have ";

            generalDictOut = values[1].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ". Upon closer inspection, they have ";

            generalDictOut = values[2].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ", they have ";

            generalDictOut = values[3].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += " and they have ";

            generalDictOut = values[4].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ". Their mouth has ";

            generalDictOut = values[6].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ", they also have ";

            generalDictOut = values[7].ElementAt(0).Key;
            description += generalDictOut.ToLower();

            description += ".";

            if (values[9] != null)
            {
                description += " Lastly, they have ";

                generalDictOut = values[9].ElementAt(0).Key;
                description += generalDictOut.ToLower();

            }
            
            Dictionary<string, string> retValue = new Dictionary<string, string>();
            retValue.Add(name, description);
            return retValue;
        }
        //Read from database and store into a dictionary holding specific values. column descriptions in db are the key for the dictionary and the info in that column is the data in the dictionary
        
        public Dictionary<string, string> Query(DataSet data)
        {
            Dictionary<string, string> dataValues = new Dictionary<string, string>();

            DataRowCollection rowValue = data.Tables[0].Rows;
            DataColumnCollection colValue = data.Tables[0].Columns;

            foreach (DataRow row in rowValue)
            {
                int i = 0;
                foreach (object obj in row.ItemArray)
                {
                    dataValues.Add(obj.ToString(), colValue[i++].Caption.ToString());
                }
            }

            return dataValues;

        }
        //loads in user specified data, queries db for it and then puts it into correct formatting to be returned
        public Dictionary<string, string> NpcQuery(List<string> userSpecifiedData)
        {
            //userSpecifiedData[0] = race, [1] = gender, [2] = occupation
            var rand = new Random();

            //This order is the same as the order in database
            List<string> queries = new List<string>();
            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();

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

            foreach (string queryStrings in queries)
            {
                values.Add(Query(QueryDatabase("..\\..\\sqlDatabase\\NPC.db", queryStrings)));
            }
            Dictionary<string, string> returnValue = NpcFormat(values, name, userSpecifiedData[0], gender);

            return returnValue;

        }
        #endregion

    }
}
