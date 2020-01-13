using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.RandomTables
{
    class RandomGeneratorModel : INotifyPropertyChanged
    {
        class RandomTableCategory
        {
            string name;
            ObservableCollection<RandomTableCategory> subCategories = new ObservableCollection<RandomTableCategory>();

            public string Name
            { 
                get { return name; }
                set { name = value; }
            }

            public ObservableCollection<RandomTableCategory> SubCategories
            {
                get { return subCategories; }
                set { subCategories = value; }
            }
        }

        #region Members
        private ObservableCollection<string> randomTableResult = new ObservableCollection<string>();

        private bool latestGenerationSaved;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public ObservableCollection<string> RandomTableResults
        {
            get { return randomTableResult; }
        }

        public bool GenerationNotSaved
        {
            get { return !latestGenerationSaved; }
        }
        #endregion

        #region Constructor
        public RandomGeneratorModel()
        {
            latestGenerationSaved = true;
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
