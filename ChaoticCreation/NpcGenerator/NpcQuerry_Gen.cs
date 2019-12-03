using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoticCreation.NpcGenerator
{
    class NpcQuerry_Gen : GeneralGenerator
    {
        #region Members
        private string query = "SELECT occ.OccName, q.QualityDescription, i.ItemDescription, a.ActionDescription FROM Occupation occ LEFT JOIN Quality q ON q.QualityID == occ.OccQuality INNER JOIN OccItems oi ON oi.OccID == occ.OccID INNER JOIN Items i ON i.ItemID == oi.ItemID INNER JOIN OccAction oa ON oa.OccID == occ.OccID INNER JOIN Actions a ON a.ActionID == oa.ActionID ORDER BY RANDOM() LIMIT 1;";
        private string colorQuery = "SELECT c.Color FROM Color c ORDER BY RANDOM() LIMIT 1;";
        private string raceQuery = "SELECT Race.RaceAdj FROM Race WHERE Race.Race == LOWER('Gnome')";

        //actions
        private int actionID;
            //string?
        private int actionDescription;
        
        //color
        private int colorID;
        private int color;
        private int colorAdj;

        //descriptor
        private int descriptorID;
        private string description;
        private string gender;

        //items
        private List<int> itemID;
        private List<int> itemDescription;

        //names
        private int nameID;
        private string name;
        //private string gender;

        //occAction
        //private int occID;
        //private int actionID;

        //occItems
        //private int occID;
        //private int itemID;

        //occupation
        private int occID;
        private string occName;
        private int occQuality;

        //quality
        private int qualityID;
        private string qualityDescription;
        
        //race
        private int raceID;
        private string race;
        private string raceAdj;
        #endregion

        #region Getters and Setters
        public string CurrantQuery {
            get { return CurrantQuery; }
            set { query = value; }
        }
        public int CurrantActionID {
            get { return CurrantActionID; }
            set { actionID = value; }
        }
        public int CurrentActionDescription {
            get { return CurrentActionDescription; }
            set { actionDescription = value; }
        }
        public int CurrentColorID {
            get { return CurrentColorID; }
            set { colorID = value; }
        }
        public int CurrentColor {
            get { return CurrentColor; }
            set { color = value; }
        }
        public int CurrentColorAdj {
            get { return CurrentColorAdj; }
            set { colorAdj = value; }
        }
        public int CurrentDescriptorID {
            get { return CurrentDescriptorID; }
            set { descriptorID = value; }
        }
        public string CurrentDescription {
            get { return CurrentDescription; }
            set { description = value; }
        }
        public string CurrentGender {
            get { return CurrentGender; }
            set { gender = value; }
        }
        public List<int> CurrentItemID {
            get { return CurrentItemID; }
            set { itemID = value; }
        }
        public List<int> CurrentItemDescription {
            get { return CurrentItemDescription; }
            set { itemDescription = value; }
        }
        public int CurrentNameID {
            get { return CurrentNameID; }
            set { nameID = value; }
        }
        public string CurrentName {
            get { return CurrentName; }
            set { name = value; }
        }
        public int CurrentOccID {
            get { return CurrentOccID; }
            set { occID = value; }
        }
        public string CurrentOccName {
            get { return CurrentOccName; }
            set { occName = value; }
        }
        public int CurrentOccQuality {
            get { return CurrentOccQuality; }
            set { occQuality = value; }
        }
        public int CurrentQualityID {
            get { return CurrentQualityID; }
            set { qualityID = value; }
        }
        public string CurrentQualityDescription {
            get { return CurrentQualityDescription; }
            set { qualityDescription = value; }
        }
        public int CurrentRaceID {
            get { return CurrentRaceID; }
            set { raceID = value; }
        }
        public string CurrentRace {
            get { return CurrentRace; }
            set { race = value; }
        }
        public string CurrentRaceAdj {
            get { return CurrentRaceAdj; }
            set { raceAdj = value; }
        }
        #endregion

        #region Methods
        public Dictionary<string, string> NpcQuery(List<string> userSpecifiedData)
        {
            foreach (string userData in userSpecifiedData)
            {
                query = QueryEdit("occ.OccName", userSpecifiedData[0], query);
            }
            //Console.WriteLine(query);
            Dictionary<string, string> data = QueryDatabase("..\\..\\sqlDatabase\\NPC.db", query);

            return data;

        }
        #endregion

    }
}
