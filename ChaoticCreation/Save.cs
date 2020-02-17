using System;
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
    class Save
    {
        public void Creation(Creation creation)
        {

            string description = "";

            if(creation.Type == GeneratorTypesEnum.Encounter)
            {
                description = createEncounterDescription(creation.Generation);
            }
            else
            {
                description = creation.Generation["description"];
            }


            string insert = "INSERT INTO SavedCreationsTbl (CreationName, CreationDescription, CreationType)" +
                "VALUES " +
                "(" + creation.Name + ", " + description + ", " + creation.Type + ");";

            string temp = "Data Source=";
            temp += "..\\sqlDatabase\\MasterDB.db";
            temp += ";Version=3;New=False;Compress=False";

            SQLiteConnection db = new SQLiteConnection(temp);
            db.Open();
            var cmd = new SQLiteCommand(db);
            cmd.CommandText = insert;
            cmd.ExecuteNonQuery();
            db.Close();
        }

        string createEncounterDescription(Dictionary<String, String> encounter)
        {
            return string.Join(";", encounter.Select(monster => monster.Key + "," + monster.Value).ToArray());
        }
    }
}
