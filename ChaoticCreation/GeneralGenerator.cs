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
        
        protected string QueryEdit(string first, string fromFrontEnd, string query)
        {
            string temp;

            temp = " WHERE ";
            temp += first;
            temp += " == ";
            temp += fromFrontEnd;
            temp += " ";

            return query.Insert(query.LastIndexOf("ORDER BY"), temp);
        }
        protected Dictionary<string, string> QueryDatabase(string filePath, string query) 
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

            Dictionary<string, string> returnValue = new Dictionary<string, string>();

            foreach (DataTable table in dataSet.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    List<string> xComp = new List<string>();
                    List<string> yComp = new List<string>();

                    foreach (object y in row.ItemArray)
                    {
                        yComp.Add(y.ToString());
                    }
                    foreach (DataColumn x in table.Columns)
                    {
                        xComp.Add(x.ToString());
                    }
                    Console.WriteLine(xComp.Count);
                    for (int i = 0; i < xComp.Count; i++)
                    {
                        //x and y comp should be the same size
                        //Console.WriteLine(xComp[i]);
                        returnValue.Add(xComp[i], yComp[i]);
                    }
                }
            }

            

            return returnValue;
        }
        
        #endregion
    }
}
