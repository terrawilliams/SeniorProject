using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.NpcGenerator
{
    class NpcDetails
    {
        #region Members
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

    }
}
