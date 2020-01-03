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

        //edit the query so that we can have searches
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
        
        #endregion
    }
}
