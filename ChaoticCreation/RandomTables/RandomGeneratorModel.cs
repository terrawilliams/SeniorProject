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
        private ObservableCollection<string> currentTableEntries = new ObservableCollection<string>();

        private RandomTableCategory listOfTables = new RandomTableCategory("Random Tables");

        private ObservableCollection<string> randomTableResult = new ObservableCollection<string>();

        private bool latestGenerationSaved;

        private RandomTables_Gen randomTables_Gen = new RandomTables_Gen();

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

        public ObservableCollection<string> CurrentTableEntries
        {
            get { return currentTableEntries; }
        }

        public ObservableCollection<string> RandomTableResults
        {
            get { return randomTableResult; }
        }

        public bool GenerationNotSaved
        {
            get { return !latestGenerationSaved; }
        }

        public RandomTableCategory ListOfTables 
        {
            get { return listOfTables; }
        }
        #endregion

        #region Constructor
        public RandomGeneratorModel()
        {
            latestGenerationSaved = true;

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
                string newEntry = entry.lower + ((entry.lower == entry.lower)? "":(" - " + entry.upper)) + "\t" + entry.description;

                currentTableEntries.Add(newEntry);
            }
        }

        public void GenerateRandomTableSelection()
        {
            Console.WriteLine("Generate Random Table Selection Button Pressed");

            latestGenerationSaved = false;
            OnPropertyChanged("GenerationNotSaved");
        }

        public void SaveRandomTableSelection()
        {
            if (!latestGenerationSaved)
            {
                Console.WriteLine("Save Random Table Selection Button Pressed");
                latestGenerationSaved = true;
                OnPropertyChanged("GenerationNotSaved");
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
