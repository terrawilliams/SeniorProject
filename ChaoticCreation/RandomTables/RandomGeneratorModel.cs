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
        private int dieResult;
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

        public int DieResult
        {
            get { return dieResult; }
        }

        public string DieResultPercentile
        {
            get
            {
                if (dieResult <= 10)
                    return "00";
                else if (dieResult % 10 == 0)
                    return (dieResult - 10).ToString();
                else
                    return (dieResult - (dieResult % 10)).ToString(); 
            }
        }

        public int DieResultD10
        {
            get { return dieResult % 10; }
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

        public bool IsD4
        {
            get { return (dieToRoll == 4); }
        }

        public bool IsD6
        {
            get { return (dieToRoll == 6); }
        }

        public bool IsD8
        {
            get { return (dieToRoll == 8); }
        }

        public bool IsD10
        {
            get { return (dieToRoll == 10); }
        }

        public bool IsD12
        {
            get { return (dieToRoll == 12); }
        }

        public bool IsD20
        {
            get { return (dieToRoll == 20); }
        }

        public bool IsD100
        {
            get { return (dieToRoll == 100); }
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
            selectedEntry = new KeyValuePair<string, string>();
            OnPropertyChanged("SelectedEntry");

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

            UpdateDieToRoll();
        }

        public void GenerateRandomTableSelection()
        {
            if (dieToRoll == 0)
                return;

            dieResult = rand.Next(1, dieToRoll + 1);
            OnPropertyChanged("DieResult");
            OnPropertyChanged("DieResultD10");
            OnPropertyChanged("DieResultPercentile");

            string selectedDescription = string.Empty;

            foreach (RandomTableEntry entry in currentTableContents)
            {
                if (entry.lower <= dieResult && entry.upper >= dieResult)
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
            if (dieToRoll == 0)
                return;

            PreliminaryRoll();

            System.Threading.Thread.Sleep(250);

            PreliminaryRoll();

            System.Threading.Thread.Sleep(250);

            PreliminaryRoll();

            System.Threading.Thread.Sleep(250);

            GenerateRandomTableSelection();
        }

        private void PreliminaryRoll()
        {
            dieResult = rand.Next(1, dieToRoll + 1);
            OnPropertyChanged("DieResult");
            OnPropertyChanged("DieResultD10");
            OnPropertyChanged("DieResultPercentile");
        }

        private void UpdateDieToRoll()
        {
            OnPropertyChanged("DieToRoll");
            OnPropertyChanged("IsD4");
            OnPropertyChanged("IsD6");
            OnPropertyChanged("IsD8");
            OnPropertyChanged("IsD10");
            OnPropertyChanged("IsD12");
            OnPropertyChanged("IsD20");
            OnPropertyChanged("IsD100");
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
