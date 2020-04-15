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
            InitializeComponent();
            this.DataContext = savedCreationsModel;
        }

        private void savedCreationsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            savedCreationsModel.SelectedCreationName = savedCreationsTree.SelectedValue.ToString();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            savedCreationsModel.DeleteSelectedCreation();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            savedCreationsModel.EditCreation();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            savedCreationsModel.StopEditingCreation();
        }

        private void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            savedCreationsModel.SaveNewVersionOfCreation(NameTextBox.Text, DescriptionTextBox.Text);
            savedCreationsModel.StopEditingCreation();
        }
    }
}
