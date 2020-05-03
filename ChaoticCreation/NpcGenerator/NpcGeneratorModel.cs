using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.NpcGenerator
{
    class NpcGeneratorModel : INotifyPropertyChanged
    {
        #region Members
        private List<string> npcRace = new List<string>();
        private List<string> npcGender = new List<string>();
        private List<string> npcOccupation = new List<string>();

        private string currentNpcRace;
        private string currentNpcGender;
        private string currentNpcOccupation;

        private string npcName;
        private string npcDescription;

        private Dictionary<string, string> currentNpc = new Dictionary<string, string>();

        private bool latestGenerationSaved;

        NpcQuery_Gen generator = new NpcQuery_Gen();

        private Random rand = new Random();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public List<string> NpcRace
        {
            get { return npcRace; }
        }
        public List<string> NpcGender
        {
            get { return npcGender; }
        }
        public List<string> NpcOccupation
        { 
            get { return npcOccupation; }
        }        

        public string CurrentNpcRace
        {
            get { return currentNpcRace; }
            set { currentNpcRace = value; }
        }
        public string CurrentNpcGender
        { 
            get { return currentNpcGender; }
            set { currentNpcGender = value; }
        }
        public string CurrentNpcOccupation
        {
            get { return currentNpcOccupation; }
            set { currentNpcOccupation = value; }
        }
        public string NpcName
        {
            get { return npcName; }
            set 
            {
                npcName = value;
                OnPropertyChanged("NpcName");
            }
        }
        public string NpcDescription
        {
            get { return npcDescription; }
            set
            {
                npcDescription = value;
                OnPropertyChanged("NpcDescription");
            }
        }

        public bool GenerationNotSaved
        {
            get { return !latestGenerationSaved; }
        }
        #endregion

        #region Constructor
        public NpcGeneratorModel()
        {
            latestGenerationSaved = true;

            npcOccupation.Add("Any");

            NpcQuery_Gen DatabaseToUI = new NpcQuery_Gen();
            var dictionary = DatabaseToUI.Query(DatabaseToUI.QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", "SELECT * FROM Occupations ORDER BY OccName ASC;"));
            foreach (KeyValuePair<string,string> iterate in dictionary)
            {
                npcOccupation.Add(iterate.Key);
            }


            npcRace.Add("Any");
            NpcQuery_Gen DatabaseToUI2 = new NpcQuery_Gen();
            var dictionary2 = DatabaseToUI.Query(DatabaseToUI.QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", "SELECT Race FROM NPCRaces ORDER BY Race ASC;"));
            foreach (KeyValuePair<string, string> iterate in dictionary2)
            {
                npcRace.Add(iterate.Key);
            }

            npcGender.Add("Any");
            npcGender.Add("Male");
            npcGender.Add("Female");

            currentNpcRace = npcRace.First();
            currentNpcGender = npcGender.First();
            currentNpcOccupation = npcOccupation.First();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Assigns discrete values to any parameters left as "Any" then generates a NPC that satisfies the given parameters
        /// </summary>
        public void GenerateNewNpc()
        {
            string race = (currentNpcRace.Equals("Any") ? npcRace.ElementAt(rand.Next(1, npcRace.Count)): currentNpcRace);
            string gender = (currentNpcGender.Equals("Any") ? npcGender.ElementAt(rand.Next(1, npcGender.Count)) : currentNpcGender);
            string occupation = currentNpcOccupation;

            Dictionary<string,string> generationArguments = new Dictionary<string,string>();
            generationArguments["race"] = race;
            generationArguments["gender"] = gender;
            generationArguments["occupation"] = occupation;

            currentNpc = generator.NpcQuery(generationArguments);
            
            NpcName = currentNpc["name"];
            NpcDescription = currentNpc["description"];

            RecentCreations.Instance.AddCreation(NpcName, GeneratorTypesEnum.NPC, currentNpc);

            latestGenerationSaved = false;
            OnPropertyChanged("GenerationNotSaved");
        }

        /// <summary>
        /// Saves the currently generated NPC to the database if it has not already been saved
        /// </summary>
        public void SaveCurrentNpc()
        {
            if (!latestGenerationSaved)
            {
                latestGenerationSaved = true;
                OnPropertyChanged("GenerationNotSaved");

                Creation savedCreation = new Creation(NpcName, GeneratorTypesEnum.NPC, currentNpc);

                Save.Instance.Creation(savedCreation);
            }
        }

        /// <summary>
        /// Alerts the front end that a property has changed and needs to be updated
        /// </summary>
        /// <param name="property">The property that needs to be updated</param>
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
