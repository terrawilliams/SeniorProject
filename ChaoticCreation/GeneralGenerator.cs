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
        /// <summary>
        ///     Adapts the query string to the specific requirements for any special query from any generator
        /// </summary>
        /// <param name="table"></param>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <param name="search"></param>
        /// <returns>
        ///     Returns the adapted query string
        /// </returns>
        /// <special instructions>
        ///     can adapt to many situations
        ///     must require at least the table being queried, then it default uses the general query defined in the members of this class, however that can be replaced if need be(see the UI implementation in NpcGeneratorModel)
        ///     column and search are for when you are searching for something specfic (user specified) column is the column in the table you want to grab from, and search is the specific thing you are searching for
        /// </special instructions>
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
        /// <summary>
        ///     Runs the query into the database to result in a SQL dataset that can be used by the generators
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="query"></param>
        /// <returns>
        ///     The SQL dataset that can be edited and observed by any generator
        /// </returns>
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
        /// <summary>
        ///     Read from database and store into a dictionary holding specific values.
        ///     Column descriptions in db are the key for the dictionary and the info in that column is the data in the dictionary.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        ///     The final dictionary containing strings for both the key and data. Used by the UI connection and generators.
        /// </returns>

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
