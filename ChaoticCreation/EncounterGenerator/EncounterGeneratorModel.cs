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

        private string encounterDescription; //description returned to front end
        public event PropertyChangedEventHandler PropertyChanged;

        private Random rand = new Random();

        private ObservableCollection<string> encounterMonsters = new ObservableCollection<string>();
       
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

        public ObservableCollection<string> Monsters
        {
            get { return encounterMonsters; }
            set { encounterMonsters = value; }
        }

        #endregion

        #region Constructor
        public EncounterGeneratorModel()
        {
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

            //IF NO RESULTS, NEED TO RESEND QUERY
            Dictionary<string, string> generatedEncounter = generator.EncounterQuery(arguments);

            /*
            Console.WriteLine("Test returned dictionary:");
            foreach (KeyValuePair<string, string> kvp in generatedEncounter)
            {
                Console.WriteLine("Key = " + kvp.Key);
                Console.WriteLine("Value = " + kvp.Value);
            }
            */
            

            /*
             * put code here to add the generated monsters to the encounterMonsters observable collection
            */

            string encounterName = chosenDifficulty + " Level " + chosenPartyLevel + " " + chosenTerrain + " Encounter";

            RecentCreations.Instance.AddCreation(encounterName, GeneratorTypesEnum.Encounter, generatedEncounter);

            Console.WriteLine("Generate Encounter Button Pressed");
        }

        public void SaveCurrentEnocunter()
        {
            Console.WriteLine("Save Encounter Button Clicked");
        }
        
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
