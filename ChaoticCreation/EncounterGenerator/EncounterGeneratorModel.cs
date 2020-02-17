using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.EncounterGenerator
{
    class EncounterGeneratorModel : INotifyPropertyChanged
    {
        #region Members
        private List<int> partySize = new List<int>();
        private List<int> partyLevel = new List<int>();
        private List<string> terrain = new List<string>();
        private List<string> difficulty = new List<string>();

        private int currentPartySize;
        private int currentPartyLevel;
        private string currentTerrain;
        private string currentDifficulty;

        private bool latestGenerationSaved;

        private string encounterDescription; //description returned to front end

        public event PropertyChangedEventHandler PropertyChanged;

        private Random rand = new Random();

        private ObservableCollection<KeyValuePair<string, string>> encounterMonsters = new ObservableCollection<KeyValuePair<string, string>>();
       
        #endregion

        #region Getters and Setters
        public List<int> PartySize
        {
            get { return partySize; }
        }
        public List<int> PartyLevel
        {
            get { return partyLevel; }
        }
        public List<string> Terrain
        {
            get { return terrain; }
        }
        public List<string> Difficulty
        {
            get { return difficulty; }
        }

        public int SelectedPartySize
        {
            get { return currentPartySize; }
            set { currentPartySize = value; }
        }
        public int SelectedPartyLevel
        {
            get { return currentPartyLevel; }
            set { currentPartyLevel = value; }
        }
        public string SelectedTerrain
        {
            get { return currentTerrain; }
            set { currentTerrain = value; }
        }
        public string SelectedDifficulty
        {
            get { return currentDifficulty; }
            set { currentDifficulty = value; }
        }

        public string EncounterDescription
        {
            get { return encounterDescription; }
            set
            {
                encounterDescription = value;
                OnPropertyChanged("EncounterDescription");
            }
        }

        public ObservableCollection<KeyValuePair<string, string>> Monsters
        {
            get { return encounterMonsters; }
            set { encounterMonsters = value; }
        }

        public bool GenerationNotSaved
        {
            get { return !latestGenerationSaved; }
        }
        #endregion

        #region Constructor
        public EncounterGeneratorModel()
        {
            latestGenerationSaved = true;

            for(int i = 1; i <= 20; i++)
            {
                partySize.Add(i);
                partyLevel.Add(i);
            }

            terrain.Add("Any");
            terrain.Add("Arctic");
            terrain.Add("Coastal");
            terrain.Add("Desert");
            terrain.Add("Forest");
            terrain.Add("Grassland");
            terrain.Add("Hill");
            terrain.Add("Mountain");
            terrain.Add("Swamp");
            terrain.Add("Underdark");
            terrain.Add("Underwater");
            terrain.Add("Urban");

            difficulty.Add("Any");
            difficulty.Add("Easy");
            difficulty.Add("Medium");
            difficulty.Add("Hard");
            difficulty.Add("Deadly");

            currentPartySize = partySize.First();
            currentPartyLevel = partyLevel.First();
            currentTerrain = terrain.First();
            currentDifficulty = difficulty.First();
        }
        #endregion

        public void GenerateEncounter()
        {
            encounterMonsters.Clear();

            string chosenPartySize = currentPartySize.ToString();
            string chosenPartyLevel = currentPartyLevel.ToString();
            string chosenTerrain = (currentTerrain.Equals("Any") ? terrain.ElementAt(rand.Next(1, terrain.Count)) : currentTerrain);
            string chosenDifficulty = (currentDifficulty.Equals("Any") ? difficulty.ElementAt(rand.Next(1, difficulty.Count)) : currentDifficulty);

            //List of user input from UI
            List<string> arguments = new List<string>();
            arguments.Add(chosenPartySize);
            arguments.Add(chosenPartyLevel);
            arguments.Add(chosenTerrain);
            arguments.Add(chosenDifficulty);

            //Send list to Query Generator
            EncounterQuery_Gen generator = new EncounterQuery_Gen();

            //Key = monsterName, Value = count of monster
            Dictionary<string, string> generatedEncounter = generator.EncounterQuery(arguments);

            foreach( KeyValuePair<string, string> monster in generatedEncounter)
            {
                encounterMonsters.Add(monster); //Leah changed from Value to Key
            }

            string encounterName = chosenDifficulty + " Level " + chosenPartyLevel + " " + chosenTerrain + " Encounter";

            RecentCreations.Instance.AddCreation(encounterName, GeneratorTypesEnum.Encounter, generatedEncounter);

            Console.WriteLine("Generate Encounter Button Pressed");

            latestGenerationSaved = false;
            OnPropertyChanged("GenerationNotSaved");
        }

        public void SaveCurrentEnocunter()
        {
            if (!latestGenerationSaved)
            {
                Console.WriteLine("Save Encounter Button Clicked");
                latestGenerationSaved = true;
                OnPropertyChanged("GenerationNotSaved");
            }
        }
        
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
