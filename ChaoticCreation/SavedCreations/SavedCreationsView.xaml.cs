using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChaoticCreation.SavedCreations
{
    /// <summary>
    /// Interaction logic for SavedCreationsView.xaml
    /// </summary>
    public partial class SavedCreationsView : UserControl
    {
        private SavedCreationsModel savedCreationsModel = new SavedCreationsModel();
        public SavedCreationsView()
        {
            this.DataContext = savedCreationsModel;

            InitializeComponent();
        }

        private void savedCreationsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
