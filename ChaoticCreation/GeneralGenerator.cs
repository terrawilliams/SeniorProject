using System;
using System.Collections.Generic;

//sql
using System.Data.SQLite;
//dataset
using System.Data;

namespace ChaoticCreation
{
    public class GeneralGenerator
    {

        #region Constructors
        public GeneralGenerator()
        {
        }

        #endregion

        #region Members
        private string fileName;
        public const string GeneralQuery = "SELECT * FROM table ORDER BY RANDOM() LIMIT 1;";

        #endregion

        #region Getters and Setters
        public string CurrentFileName {
            get { return CurrentFileName; }
            set { fileName = value; }
        }
        #endregion

        #region Methods

        //can adapt to many situations
        //must require at least the table being queried, then it default uses the general query defined in the members of this class, however that can be replaced if need be(see the UI implementation in NpcGeneratorModel)
        //column and search are for when you are searching for something specfic (user specified) column is the column in the table you want to grab from, and search is the specific thing you are searching for
        public string QueryEdit(string table, string query = GeneralQuery, string column = null, string search = null)
        {
            string retValue = query;

            if (search != null)
            {
                string temp;

                temp = " WHERE ";
                temp += column;
                temp += " == '";
                temp += search;
                temp += "' ";
                retValue = query.Insert(query.LastIndexOf("ORDER BY"), temp);

            }

            retValue = retValue.Replace("table", table);
            return retValue;

        }

        //query the database
        public DataSet QueryDatabase(string filePath, string query) 
        {
            string temp = "Data Source=";
            temp += filePath;
            temp += ";Version=3;New=False;Compress=False";

            SQLiteConnection db = new SQLiteConnection(temp);
            db.Open();

            SQLiteDataAdapter data = new SQLiteDataAdapter(query, db);
            DataSet dataSet = new DataSet();

            dataSet.Reset();
            data.Fill(dataSet);

            return dataSet;
        }
        //Read from database and store into a dictionary holding specific values. column descriptions in db are the key for the 
        //dictionary and the info in that column is the data in the dictionary
        public Dictionary<string, string> Query(DataSet data)
        {
            Dictionary<string, string> dataValues = new Dictionary<string, string>();

            Console.WriteLine(data.Tables.Count);

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
        #endregion
    }
}
