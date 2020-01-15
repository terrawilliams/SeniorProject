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

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Getters and Setters
        public ObservableCollection<Creation> MostRecentCreations
        {
            get { return RecentCreations.MostRecentCreations; }
        }

        public Creation SelectedRecentCreation
        {
            set 
            { 
                selectedCreation = value;
                OnPropertyChanged("SelectedRecentCreation");

                if(selectedCreation.Type == GeneratorTypesEnum.NPC || selectedCreation.Type == GeneratorTypesEnum.Location)
                {
                    selectedCreationContent = selectedCreation.Generation["description"];
                    OnPropertyChanged("SelectedCreationContent");
                }
                else if(selectedCreation.Type == GeneratorTypesEnum.Encounter)
                {
                    selectedCreationContent = string.Empty;

                    foreach(KeyValuePair<string, string> monster in selectedCreation.Generation)
                    {
                        selectedCreationContent += monster.Key + " " + monster.Value + "\n";
                        OnPropertyChanged("SelectedCreationContent");
                    }
                }
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
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

}
