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
            IsVisible = true;
        }

        public string Name { get; set; }
        public ObservableCollection<RandomTableCategory> SubCategories { get; set; }
        public bool IsVisible { get; set; }
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
        private RandomTableCategory trimmedListOfTables = new RandomTableCategory("Trimmed Random Tables");

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

        public RandomTableCategory TrimmedListOfTables
        {
            get { return trimmedListOfTables; }
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

            if (currentTable == null) return;

            currentTableContents = randomTables_Gen.GetTable(currentTable);

            foreach (RandomTableEntry entry in currentTableContents)
            {
                string range = entry.lower + ((entry.lower == entry.upper) ? "" : (" - " + entry.upper));
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

            foreach (RandomTableEntry entry in currentTableContents)
            {
                if (entry.lower <= dieRoll && entry.upper >= dieRoll)
                {
                    selectedDescription = entry.description;
                    break;
                }
            }

            foreach (KeyValuePair<string, string> entry in currentTableEntries)
            {
                if (entry.Value == selectedDescription)
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

        public void TrimTablesTree(string filter)
        {
            
            if (filter == string.Empty)
            {
                trimmedListOfTables = listOfTables;
            }
            else
            {
                trimmedListOfTables.SubCategories.Clear();
                foreach(RandomTableCategory category in listOfTables.SubCategories)
                {
                    if (category.SubCategories.Count == 0)
                    {
                        if (category.Name.ToLower().Contains(filter.ToLower()))
                            trimmedListOfTables.SubCategories.Add(category);
                    }
                    else
                    {
                        trimmedListOfTables.SubCategories.Add(TrimTablesTreeHelper(category, filter));
                    }
                }
            }
        }

        public RandomTableCategory TrimTablesTreeHelper(RandomTableCategory currentNode, string filter)
        {
            RandomTableCategory newCategory = new RandomTableCategory(currentNode.Name);

            foreach(RandomTableCategory node in currentNode.SubCategories)
            {
                if(node.SubCategories.Count == 0)
                {
                    if(node.Name.ToLower().Contains(filter.ToLower()))
                    {
                        newCategory.SubCategories.Add(node);
                    }
                }
                else
                {
                    newCategory.SubCategories.Add(TrimTablesTreeHelper(node, filter));
                }
            }

            return newCategory;
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
