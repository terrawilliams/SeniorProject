using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.HomeTab
{
    class HomeTabModel : INotifyPropertyChanged
    {
        #region Members
        private Creation selectedCreation;
        private string selectedCreationContent = string.Empty;
        private bool creationSaved = true;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public ObservableCollection<Creation> MostRecentCreations
        {
            get { return RecentCreations.MostRecentCreations; }
        }

        public bool CreationNotSaved
        {
            get { return !creationSaved; }
        }

        public Creation SelectedRecentCreation
        {
            set 
            { 
                selectedCreation = value;
                OnPropertyChanged("SelectedRecentCreation");

                DisplaySelectedCreation();

                creationSaved = CurrentCreationIsSaved();
                OnPropertyChanged("CreationNotSaved");
            }
            get { return selectedCreation; }
        }

        public string SelectedCreationContent
        {
            get { return selectedCreationContent; }
        }
        #endregion

        #region Constructor
        public HomeTabModel()
        {

        }
        #endregion

        #region Methods
        public void SaveCurrentCreation()
        {
            if(!creationSaved)
            {
                Save.Instance.Creation(selectedCreation);
                creationSaved = true;
                OnPropertyChanged("CreationNotSaved");
            }
        }

        private void DisplaySelectedCreation()
        {
            if (selectedCreation.Type == GeneratorTypesEnum.NPC || selectedCreation.Type == GeneratorTypesEnum.Location)
            {
                selectedCreationContent = selectedCreation.Generation["description"];
                OnPropertyChanged("SelectedCreationContent");
            }
            else if (selectedCreation.Type == GeneratorTypesEnum.Encounter)
            {
                selectedCreationContent = string.Empty;

                foreach (KeyValuePair<string, string> monster in selectedCreation.Generation)
                {
                    selectedCreationContent += monster.Key + " " + monster.Value + "\n";
                    OnPropertyChanged("SelectedCreationContent");
                }
            }
        }

        private bool CurrentCreationIsSaved()
        {
            bool saved = false;

            if(selectedCreation.Type == GeneratorTypesEnum.NPC)
            {
                foreach(string npc in Save.SavedNpcNames)
                {
                    if (npc == selectedCreation.Name)
                        saved = true;
                }
            }
            else if(selectedCreation.Type == GeneratorTypesEnum.Location)
            {
                foreach (string location in Save.SavedLocationNames)
                {
                    if (location == selectedCreation.Name)
                        saved = true;
                }
            }
            else if(selectedCreation.Type == GeneratorTypesEnum.Encounter)
            {
                foreach (string encounter in Save.SavedEncounterNames)
                {
                    if (encounter == selectedCreation.Name)
                        saved = true;
                }
            }

            return saved;
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

}
