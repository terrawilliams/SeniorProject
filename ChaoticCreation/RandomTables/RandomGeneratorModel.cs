using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.RandomTables
{
    class RandomGeneratorModel
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
        #endregion

        #region Getters and Setters
        public ObservableCollection<string> RandomTableResults
        {
            get { return randomTableResult; }
        }
        #endregion

        #region Constructor
        public RandomGeneratorModel()
        {

        }
        #endregion

        public void GenerateRandomTableSelection()
        {
            Console.WriteLine("Generate Random Table Selection Button Pressed");
        }

        public void SaveRandomTableSelection()
        {
            Console.WriteLine("Save Random Table Selection Button Pressed");
        }

        public void RollDie()
        {
            Console.WriteLine("Roll Die Button Pressed");
        }
    }
}
