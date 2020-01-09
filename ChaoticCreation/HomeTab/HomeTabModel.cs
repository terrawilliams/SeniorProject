using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.HomeTab
{
    class HomeTabModel
    {
        #region Members
        private string applicationDescription;
        #endregion

        #region Getters and Setters
        public ObservableCollection<Creation> MostRecentCreations
        {
            get { return RecentCreations.MostRecentCreations; }
        }

        public string ApplicationDescription
        {
            get { return applicationDescription; }
            set { applicationDescription = value; }
        }
        #endregion

        #region Constructor
        public HomeTabModel()
        {
            applicationDescription = "Chaotic Creations is a website made for Dungeon Masters of the fifth edition of Dungeons and Dragons...";
        }
        #endregion
    }
}
