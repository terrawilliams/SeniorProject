using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.RandomTables
{
    class RandomTableCategory
    {
        public RandomTableCategory(string newName)
        {
            Name = newName;
            SubCategories = new ObservableCollection<RandomTableCategory>();
        }

        public string Name { get; set; }
        public ObservableCollection<RandomTableCategory> SubCategories { get; set; }
    }

    class RandomGeneratorModel : INotifyPropertyChanged
    {
        #region Members
        private string currentTable;
        private List<RandomTableEntry> currentTableContents = new List<RandomTableEntry>();
        private ObservableCollection<KeyValuePair<string, string>> currentTableEntries = new ObservableCollection<KeyValuePair<string, string>>();
        private int dieToRoll;
        private KeyValuePair<string, string> selectedEntry;

        private RandomTableCategory listOfTables = new RandomTableCategory("Random Tables");

        private ObservableCollection<string> randomTableResult = new ObservableCollection<string>();

        private RandomTables_Gen randomTables_Gen = new RandomTables_Gen();

        private Random rand = new Random();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public string CurrentTable
        {
            get { return currentTable; }
            set 
            { 
                currentTable = value;
                OnPropertyChanged("CurrentTable");

                ShowSelectedTable();
            }
        }

        public ObservableCollection<KeyValuePair<string, string>> CurrentTableEntries
        {
            get { return currentTableEntries; }
        }

        public int DieToRoll
        {
            get { return dieToRoll; }
        }

        public KeyValuePair<string, string> SelectedEntry
        {
            get { return selectedEntry; }
        }

        public ObservableCollection<string> RandomTableResults
        {
            get { return randomTableResult; }
        }

        public RandomTableCategory ListOfTables 
        {
            get { return listOfTables; }
        }
        #endregion

        #region Constructor
        public RandomGeneratorModel()
        {
            listOfTables = randomTables_Gen.InitializeRandomTableList();
        }
        #endregion

        #region Methods
        private void ShowSelectedTable()
        {
            currentTableEntries.Clear();

            currentTableContents = randomTables_Gen.GetTable(currentTable);

            foreach(RandomTableEntry entry in currentTableContents)
            {
                string range = entry.lower + ((entry.lower == entry.upper)? "":(" - " + entry.upper));
                string description = entry.description;

                KeyValuePair<string, string> newEntry = new KeyValuePair<string, string>(range, description);

                currentTableEntries.Add(newEntry);
            }

            if (currentTableContents.Count > 0)
                dieToRoll = currentTableContents.Last().upper;
            else
                dieToRoll = 0;
            
            OnPropertyChanged("DieToRoll");
        }

        public void GenerateRandomTableSelection()
        {
            Console.WriteLine("Generate Random Table Selection Button Pressed");

            if (dieToRoll == 0)
                return;

            int dieRoll = rand.Next(1, dieToRoll);
            string selectedDescription = string.Empty;

            foreach(RandomTableEntry entry in currentTableContents)
            {
                if(entry.lower <= dieRoll && entry.upper >= dieRoll)
                {
                    selectedDescription = entry.description;
                    break;
                }
            }

            foreach(KeyValuePair<string, string> entry in currentTableEntries)
            {
                if(entry.Value == selectedDescription)
                {
                    selectedEntry = entry;
                    OnPropertyChanged("SelectedEntry");
                    break;
                }
            }
        }

        public void RollDie()
        {
            Console.WriteLine("Roll Die Button Pressed");
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
