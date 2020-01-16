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

        private RandomTableCategory listOfTables = new RandomTableCategory("Random Tables");

        private ObservableCollection<string> randomTableResult = new ObservableCollection<string>();

        private bool latestGenerationSaved;

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
            }
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

            //InitializeRandomTableList();
        }
        #endregion

        #region Methods

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
