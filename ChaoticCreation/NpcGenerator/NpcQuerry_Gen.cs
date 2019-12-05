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
    class NpcQuerry_Gen : GeneralGenerator
    {
        #region Members

        //search query strings
        private string occupation = "SELECT occ.OccName, q.QualityDescription, i.ItemDescription, a.ActionDescription FROM Occupation occ LEFT JOIN Quality q ON q.QualityID == occ.OccQuality INNER JOIN OccItems oi ON oi.OccID == occ.OccID INNER JOIN Items i ON i.ItemID == oi.ItemID INNER JOIN OccAction oa ON oa.OccID == occ.OccID INNER JOIN Actions a ON a.ActionID == oa.ActionID ORDER BY RANDOM() LIMIT 1;";
        private string color = "SELECT c.Color FROM Color c ORDER BY RANDOM() LIMIT 1;";
        private string race = "SELECT Race.RaceAdj FROM Race ORDER BY RANDOM() LIMIT 1;";
        private string name = "SELECT Name, d.Description FROM Names ORDER BY RANDOM();";

        #endregion

        #region Getters and Setters
        public string CurrentOccupation {
            get { return CurrentOccupation; }
            set { occupation = value; }
        }
        public string CurrantColor {
            get { return CurrantColor; }
            set { color = value; }
        }
        public string CurrantRace {
            get { return CurrantRace; }
            set { race = value; }
        }
        #endregion

        #region Methods
        //format the out dictionary
        private Dictionary<string, string> NpcFormat(Dictionary<string, string> Race, Dictionary<string, string> Occupation, string name, bool gender)
        {
            string description =
                "Before you stands a ";
            description += Race["RaceAdj"];

            //descriptor query string not done
            if (gender)
                description += " man, he is wearing ";
            else
                description += " woman, she is wearing ";

            description += Occupation["QualityDescription"].ToLower();
            //no color yet
            description += " clothing and they have ";
            description += Occupation["ItemDescription"];
            description += " they are ";
            description += Occupation["ActionDescription"];

            //Console.WriteLine(description);

            Dictionary<string, string> retValue = new Dictionary<string, string>();
            retValue.Add(name, description);
            return retValue;
        }
        //Read from database and store into a dictionary holding specific values. column descriptions in db are the key for the dictionary and the info in that column is the data in the dictionary
        private Dictionary<string, string> Query(DataSet data)
        {
            Dictionary<string, string> dataValues = new Dictionary<string, string>();

            DataRowCollection rowValue = data.Tables[0].Rows;
            DataColumnCollection colValue = data.Tables[0].Columns;

            foreach (DataRow row in rowValue)
            {
                int i = 0;
                foreach (object obj in row.ItemArray)
                {
                    dataValues.Add(colValue[i++].Caption.ToString(), obj.ToString());
                }
            }

            return dataValues;

        }
        //loads in user specified data, queries db for it and then puts it into correct formatting to be returned
        public Dictionary<string, string> NpcQuery(List<string> userSpecifiedData)
        {
            //edit based off of search
            string queryEditedRace = QueryEdit("Race.Race", userSpecifiedData[0], race);
            //gender(1) is at the end for rn
            //edit based off of search
            string queryEditedOccupation = QueryEdit("occ.OccName", userSpecifiedData[2], occupation);

            //string queryEditedName = QueryEdit("Names.Gender", userSpecifiedData[2], name);

            //actually query - race
            DataSet data = QueryDatabase("..\\..\\sqlDatabase\\NPC.db", queryEditedRace);
            Dictionary<string, string> dataValuesRace = Query(data);

            //actually query - occ
            DataSet dataOccupation = QueryDatabase("..\\..\\sqlDatabase\\NPC.db", queryEditedOccupation);
            Dictionary<string, string> dataValuesOcc = Query(dataOccupation);

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

            var rand = new Random();

            bool gender;
            string name;
            if (userSpecifiedData[1] == "M")
            {
                name = maleNames[rand.Next(0, maleNames.Count)];
                gender = true;
            }
            else
            {
                name = femaleNames[rand.Next(0, femaleNames.Count)];
                gender = false;
            }

            Dictionary<string, string> returnValue = NpcFormat(dataValuesRace, dataValuesOcc, name, gender);

            return returnValue;

        }
        #endregion

    }
}
