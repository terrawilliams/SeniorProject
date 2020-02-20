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

        private Save save = new Save();

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
            var dictionary = DatabaseToUI.Query(DatabaseToUI.QueryDatabase("..\\..\\sqlDatabase\\MasterDB.db", "SELECT * FROM Occupations;"));
            foreach (KeyValuePair<string,string> iterate in dictionary)
            {
                npcOccupation.Add(iterate.Key);
            }
            
            npcRace.Add("Any");
            npcRace.Add("dragonborn");
            npcRace.Add("dwarf");
            npcRace.Add("elf");
            npcRace.Add("gnome");
            npcRace.Add("half-elf");
            npcRace.Add("halfling");
            npcRace.Add("half-orc");
            npcRace.Add("human");
            npcRace.Add("tiefling");
            npcRace.Add("eladrin");

            npcGender.Add("Any");
            npcGender.Add("Male");
            npcGender.Add("Female");

            currentNpcRace = npcRace.First();
            currentNpcGender = npcGender.First();
            currentNpcOccupation = npcOccupation.First();
        }
        #endregion
        
        public void GenerateNewNpc()
        {
            string race = (currentNpcRace.Equals("Any") ? npcRace.ElementAt(rand.Next(1, npcRace.Count)): currentNpcRace);
            string gender = (currentNpcGender.Equals("Any") ? npcGender.ElementAt(rand.Next(1, npcGender.Count)) : currentNpcGender);
            string occupation = (currentNpcOccupation.Equals("Any") ? npcOccupation.ElementAt(rand.Next(1, npcOccupation.Count)) : currentNpcOccupation);

            List<string> generationArguments = new List<string>();
            generationArguments.Add(race);
            generationArguments.Add(gender);
            generationArguments.Add(occupation);

            currentNpc = generator.NpcQuery(generationArguments);
            
            NpcName = currentNpc["name"];
            NpcDescription = currentNpc["description"];

            RecentCreations.Instance.AddCreation(NpcName, GeneratorTypesEnum.NPC, currentNpc);

            latestGenerationSaved = false;
            OnPropertyChanged("GenerationNotSaved");
        }

        public void SaveCurrentNpc()
        {
            if (!latestGenerationSaved)
            {
                latestGenerationSaved = true;
                OnPropertyChanged("GenerationNotSaved");

                Creation savedCreation = new Creation(NpcName, GeneratorTypesEnum.NPC, currentNpc);

                save.Creation(savedCreation);
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
