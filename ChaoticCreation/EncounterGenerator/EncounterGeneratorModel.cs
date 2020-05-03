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

        private string encounterName;
        private Dictionary<string, Dictionary<string, string>> currentEncounter = new Dictionary<string, Dictionary<string, string>>();

        public event PropertyChangedEventHandler PropertyChanged;

        private Random rand = new Random();

        private EncounterQuery_Gen generator = new EncounterQuery_Gen();

        private ObservableCollection<KeyValuePair<string, string>> encounterMonsters = new ObservableCollection<KeyValuePair<string, string>>();
        private ObservableCollection<KeyValuePair<string, string>> encounterLoot = new ObservableCollection<KeyValuePair<string, string>>();

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

        public ObservableCollection<KeyValuePair<string, string>> Loot
        {
            get { return encounterLoot; }
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

        #region Methods
        /// <summary>
        /// Chooses values for any parameters left as "Any" then generates an encounter that fits all parameters
        /// </summary>
        public void GenerateEncounter()
        {
            encounterMonsters.Clear();
            encounterLoot.Clear();

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

            //Key = monsterName, Value = count of monster
            currentEncounter = generator.EncounterQuery(arguments);

            foreach( KeyValuePair<string, string> monster in currentEncounter["Monsters"])
            {
                encounterMonsters.Add(monster); //Leah changed from Value to Key
            }
            foreach(KeyValuePair<string, string> loot in currentEncounter["Loot"])
            {
                if(loot.Value != string.Empty)
                    encounterLoot.Add(loot);
            }

            encounterName = chosenDifficulty + " Level " + chosenPartyLevel + " " + chosenTerrain + " Encounter";

            RecentCreations.Instance.AddCreation(encounterName, GeneratorTypesEnum.Encounter, currentEncounter["Monsters"]);

            Console.WriteLine("Generate Encounter Button Pressed");

            latestGenerationSaved = false;
            OnPropertyChanged("GenerationNotSaved");
        }

        /// <summary>
        /// Saves current encounter if it has not already been saved
        /// </summary>
        public void SaveCurrentEnocunter()
        {
            if (!latestGenerationSaved)
            {
                latestGenerationSaved = true;
                OnPropertyChanged("GenerationNotSaved");

                Creation savedCreation = new Creation(encounterName, GeneratorTypesEnum.Encounter, currentEncounter["Monsters"]);

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
