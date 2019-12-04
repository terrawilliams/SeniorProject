using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.HomeTab
{
    struct creation
    {
        public string name { get; set; }
        public GeneratorTypesEnum generatorType { get; set; }
        public Dictionary<string, string> data { get; set; }

        public creation(string name, GeneratorTypesEnum generatorType, Dictionary<string, string> data)
        {
            this.name = name;
            this.generatorType = generatorType;
            this.data = data;
        }
    }

    class HomeTabModel
    {
        #region Members
        private ObservableCollection<creation> recentCreations = new ObservableCollection<creation>();
        private string applicationDescription;
        #endregion


        #region Getters and Setters
        public ObservableCollection<creation> RecentCreations
        {
            get { return recentCreations; }
            set { recentCreations = value; }
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
