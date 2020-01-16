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

            InitializeRandomTableList();
        }
        #endregion

        #region Methods
        private void InitializeRandomTableList()
        {
            listOfTables.SubCategories.Add(InitializeArtObjectsList());
            listOfTables.SubCategories.Add(InitializeGemstonesList());
            listOfTables.SubCategories.Add(InitializeMagicEffectsList());
            listOfTables.SubCategories.Add(InitializeMagicItemsList());
            listOfTables.SubCategories.Add(new RandomTableCategory("Trinkets"));
        }

        private RandomTableCategory InitializeArtObjectsList()
        {
            RandomTableCategory artObjects = new RandomTableCategory("Art Objects");

            artObjects.SubCategories.Add(new RandomTableCategory("25 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("250 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("750 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("2,500 GP Art Objects"));
            artObjects.SubCategories.Add(new RandomTableCategory("7,500 GP Art Objects"));

            return artObjects;
        }

        private RandomTableCategory InitializeGemstonesList()
        {
            RandomTableCategory gemstones = new RandomTableCategory("Gemstones");

            gemstones.SubCategories.Add(new RandomTableCategory("10 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("50 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("100 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("500 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("1,000 GP Gemstones"));
            gemstones.SubCategories.Add(new RandomTableCategory("5,000 GP Gemstones"));

            return gemstones;
        }

        private RandomTableCategory InitializeMagicEffectsList()
        {
            RandomTableCategory magicEffects = new RandomTableCategory("Magic Effects");

            magicEffects.SubCategories.Add(new RandomTableCategory("Potions"));
            magicEffects.SubCategories.Add(new RandomTableCategory("Necromancy"));
            magicEffects.SubCategories.Add(new RandomTableCategory("Wild Magic Surge"));

            return magicEffects;
        }

        private RandomTableCategory InitializeMagicItemsList()
        {
            RandomTableCategory magicItems = new RandomTableCategory("Magic Items");

            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table A"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table B"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table C"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table D"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table E"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table F"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table G"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table H"));
            magicItems.SubCategories.Add(new RandomTableCategory("Magic Items Table I"));

            return magicItems;
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
