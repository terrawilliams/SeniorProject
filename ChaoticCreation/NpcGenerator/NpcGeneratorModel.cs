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

        #endregion

        #region Constructor
        public NpcGeneratorModel()
        {
            npcRace.Add("Any");
            npcRace.Add("Dragonborn");
            npcRace.Add("Dwarf");
            npcRace.Add("Elf");
            npcRace.Add("Gnome");
            npcRace.Add("Half-Elf");
            npcRace.Add("Halfling");
            npcRace.Add("Half-Orc");
            npcRace.Add("Human");
            npcRace.Add("Tiefling");

            npcGender.Add("Any");
            npcGender.Add("Male");
            npcGender.Add("Female");

            npcOccupation.Add("Any");
            npcOccupation.Add("Adventurer");
            npcOccupation.Add("Artist");
            npcOccupation.Add("Bartender");
            npcOccupation.Add("Blacksmith");
            npcOccupation.Add("Guard");
            npcOccupation.Add("Innkeeper");
            npcOccupation.Add("Musician");
            npcOccupation.Add("Royalty");
            npcOccupation.Add("Shopkeep");

            currentNpcRace = npcRace.First();
            currentNpcGender = npcGender.First();
            currentNpcOccupation = npcOccupation.First();

            npcName = "ex name";
            npcDescription = "ex description";
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

            //GenerateNPC(generationArguments);

            NpcName = race;
            NpcDescription = gender + " " + occupation;
            Console.WriteLine("Generate NPC Button Pressed: " + race + " " + gender + " " + occupation);
        }

        public void SaveCurrentNpc()
        {
            Console.WriteLine("Save NPC Button Pressed");
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
