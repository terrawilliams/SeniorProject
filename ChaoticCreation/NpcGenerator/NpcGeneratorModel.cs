using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; //added on master 12/2/19
using System.ComponentModel; //added on master 12/2/19
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.NpcGenerator
{
    class NpcGeneratorModel : INotifyPropertyChanged //changed on master 12/2/19
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

        private Random rand = new Random(); //added on master 12/2
        public event PropertyChangedEventHandler PropertyChanged; //added on master 12/2
        #endregion

#region Getters and Setters
        public List<string> NpcRace{
            get { return npcRace; }
        }
        public List<string> NpcGender{
            get { return npcGender; }
        }
        public List<string> NpcOccupation{ 
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
            set {
                npcName = value;
                OnPropertyChanged("NpcName"); //added to master 12/2
            }
        }
        public string NpcDescription
        {
            get { return npcDescription; }
            set
            {
                npcDescription = value;
                OnPropertyChanged("NpcDescription"); //added to master 12/2
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
            string race = (currentNpcRace.Equals("Any") ? npcRace.ElementAt(rand.Next(1, npcRace.Count)): currentNpcRace); //added to master 12/2
            string gender = (currentNpcGender.Equals("Any") ? npcGender.ElementAt(rand.Next(1, npcGender.Count)) : currentNpcGender); //added to master 12/2
            string occupation = (currentNpcOccupation.Equals("Any") ? npcOccupation.ElementAt(rand.Next(1, npcOccupation.Count)) : currentNpcOccupation); //added to master 12/2

            List<string> generationArguments = new List<string>(); //added to master 12/2
            generationArguments.Add(race);  //added to master 12/2
            generationArguments.Add(gender); //added to master 12/2
            generationArguments.Add(occupation); //added to master 12/2

            //GenerateNPC(generationArguments); //removed on master 12/2

            NpcName = race; //changed 12/2
            NpcDescription = gender + " " + occupation; //changed 12/2
            Console.WriteLine("Generate NPC Button Pressed: " + race + " " + gender + " " + occupation); //changed on master 12/2
        
            
            //Console.WriteLine("Generate NPC Button Pressed");
            NpcQuerry_Gen generator = new NpcQuerry_Gen(); 
            List<string> userSpecifiedData = new List<string>(); 
            userSpecifiedData.Add("'Bard'"); 
            generator.NpcQuery(userSpecifiedData); 
            //Console.WriteLine(generator.NpcQuery(userSpecifiedData)["OccName"]);
        }

        public void SaveCurrentNpc()
        {
            Console.WriteLine("Save NPC Button Pressed");
        }
        
        //added to master 12/2
         private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}