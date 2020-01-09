using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.EncounterGenerator
{
    class EncounterGeneratorModel
    {
        #region Members
        private List<int> partySize = new List<int>();
        private List<int> partyLevel = new List<int>();
        private List<string> terrain = new List<string>();
        private List<string> difficulty = new List<string>();

        private ObservableCollection<string> encounterMonsters = new ObservableCollection<string>();

        private Random rand = new Random();

        private int currentPartySize;
        private int currentPartyLevel;
        private string currentTerrain;
        private string currentDifficulty;
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

        public ObservableCollection<string> Monsters
        {
            get { return encounterMonsters; }
            set { encounterMonsters = value; }
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

            List<string> arguments = new List<string>();

            arguments.Add(chosenPartySize);
            arguments.Add(chosenPartyLevel);
            arguments.Add(chosenTerrain);
            arguments.Add(chosenDifficulty);

            Dictionary<string, string> generation = new Dictionary<string, string>(); //set this equal to the generated dictionary instead

            /*
             * put code here to add the generated monsters to the encounterMonsters observable collection
            */

            string encounterName = chosenDifficulty + " Level " + chosenPartyLevel + " " + chosenTerrain + " Encounter";

            RecentCreations.Instance.AddCreation(encounterName, GeneratorTypesEnum.Encounter, generation);

            Console.WriteLine("Generate Encounter Button Pressed");
        }

        public void SaveCurrentEnocunter()
        {
            Console.WriteLine("Save Encounter Button Clicked");
        }
    }
}
