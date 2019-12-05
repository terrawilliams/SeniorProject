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
        #endregion

        #region Getters and Setters
        public string CurrentFileName {
            get { return CurrentFileName; }
            set { fileName = value; }
        }
        #endregion

        #region Methods
        //edit the query so that we can have searches
        protected string QueryEdit(string first, string fromFrontEnd, string query)
        {
            string temp;

            temp = " WHERE ";
            temp += first;
            temp += " == '";
            temp += fromFrontEnd;
            temp += "' ";

            return query.Insert(query.LastIndexOf("ORDER BY"), temp);
        }
        //query the database
        protected DataSet QueryDatabase(string filePath, string query) 
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
