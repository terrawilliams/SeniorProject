using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCreation.AboutTab
{
    class AboutModel
    {
        #region Members
        private string caption;
        private string description;
        #endregion

        #region Getters and Setters
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion

        #region Constructor
        public AboutModel()
        {
            caption = "Caption for group picture";
            description = "The team behind Chaotic Creations is a group of seniors in Computer Science at The University of Nevada...";
        }
        #endregion
    }
}
