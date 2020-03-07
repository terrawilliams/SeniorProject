﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//sql
using System.Data.SQLite;
//dataset
using System.Data;

namespace ChaoticCreation
{
    class dataEntry
    {
        public string creationName;
        public string creationType;
        public string creationDescription;
    }
    class Save : GeneralGenerator
    {
        public void Creation(Creation creation)
        {
            
            string description = "";

            if (creation.Type == GeneratorTypesEnum.Encounter)
            {
                description = createEncounterDescription(creation.Generation);
            }
            else
            {
                description = creation.Generation["description"];
            }


            string insert = "INSERT INTO SavedCreationsTbl (CreationName, CreationDescription, CreationType)" +
                "VALUES " +
                "(\"" + creation.Name + "\", \"" + description + "\", \"" + creation.Type + "\");";

            string temp = "Data Source=";
            temp += "..\\..\\sqlDatabase\\MasterDB.db";
            temp += ";Version=3;New=False;Compress=False";

            SQLiteConnection db = new SQLiteConnection(temp);
            db.Open();
            var cmd = new SQLiteCommand(db);
            cmd.CommandText = insert;
            cmd.ExecuteNonQuery();
            db.Close();

        }

        void deleteCreation(Creation to_be_deleted)
        {
            string delete = "DELETE FROM SavedCreationsTbl WHERE \"" + to_be_deleted.Name + "\" = CreationName;";

            string temp = "Data Source=";
            temp += "..\\..\\sqlDatabase\\MasterDB.db";
            temp += ";Version=3;New=False;Compress=False";

            SQLiteConnection db = new SQLiteConnection(temp);
            db.Open();
            var cmd = new SQLiteCommand(db);
            cmd.CommandText = delete;
            cmd.ExecuteNonQuery();
            db.Close();
        }

        string createEncounterDescription(Dictionary<String, String> encounter)
        {
            return string.Join(":", encounter.Select(monster => monster.Key + "," + monster.Value).ToArray());
        }

        Dictionary<String, String> RecreateEncouterDescription(string description)
        {
            Dictionary<String, String> encounter = new Dictionary<string, string>();
            String[] monsters = description.Split(':');

            foreach(string monster in monsters)
            {
                String[] temp = monster.Split(',');
                string temp_monster = temp[0];
                string temp_num = temp[1];

                encounter[temp_monster] = temp_num;
            }

            return encounter;
        }

        List<Creation> RetrieveSavedCreations()
        {

            List<dataEntry> values = new List<dataEntry>();

            values = QueryForCreations();

            List<Creation> creations = new List<Creation>(); 

            foreach(dataEntry creation in values)
            {
                string name = creation.creationName;

                //This atrocity converts the string in the database to the appropriate GeneratorTypesEnum to store in the new 
                //Creation object
                GeneratorTypesEnum type =  (GeneratorTypesEnum) Enum.Parse(typeof(GeneratorTypesEnum), creation.creationType);

                Dictionary<string, string> generation = new Dictionary<string, string>();

                if(type == GeneratorTypesEnum.Encounter)
                {
                    generation = RecreateEncouterDescription(creation.creationDescription);
                }
                else
                {
                    generation["description"] = creation.creationDescription;
                }

                creations.Add(new Creation(name, type, generation));
            }

            return creations;
        }
        List<dataEntry> QueryForCreations()
        {
            string queryString = "Select * From SavedCreationsTbl;";

            DataSet data = QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", queryString);

            List<dataEntry> entries = new List<dataEntry>();

            Console.WriteLine(data.Tables.Count);

            DataRowCollection rowValue = data.Tables[0].Rows;
            DataColumnCollection colValue = data.Tables[0].Columns;

            foreach (DataRow row in rowValue)
            {
                for(int i = 0; i < row.ItemArray.Length; i+=4)
                {
                    dataEntry temp = new dataEntry();
                    temp.creationName = row.ItemArray[i + 1].ToString();
                    temp.creationDescription = row.ItemArray[i + 2].ToString();
                    temp.creationType = row.ItemArray[i + 3].ToString();

                    entries.Add(temp);

                }
            }

            return entries;
        }

        
    }
}
