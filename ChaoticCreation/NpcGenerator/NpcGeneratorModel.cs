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

            //KEEP ONLY THESE HERE FOR NOW OR ELSE YOU BREAK THINGS
            npcOccupation.Add("Any");
            npcOccupation.Add("Academic");
            npcOccupation.Add("Apothecary");
            npcOccupation.Add("Athlete");
            npcOccupation.Add("Celebrity");
            npcOccupation.Add("Assassin");
            npcOccupation.Add("Artisan");
            //Doesn't work anything after :
            //npcOccupation.Add("Colonist");

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
            
            Dictionary<string, string> generatedNpc = new Dictionary<string, string>();
            generatedNpc.Add(race, gender + " " + occupation);
            //generatedNpc = GenerateNPC(generationArguments);

            //This is what I added @tera
            NpcQuerry_Gen generator = new NpcQuerry_Gen();
            generatedNpc = generator.NpcQuery(generationArguments);
            //end
            
            NpcName = generatedNpc.First().Key;
            NpcDescription = generatedNpc.First().Value;

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
