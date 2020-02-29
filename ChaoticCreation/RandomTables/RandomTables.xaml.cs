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

namespace ChaoticCreation.RandomTables
{
    /// <summary>
    /// Interaction logic for RandomTables.xaml
    /// </summary>
    public partial class RandomTables : UserControl
    {
        RandomGeneratorModel randomGeneratorModel = new RandomGeneratorModel();

        public RandomTables()
        {
            InitializeComponent();
            this.DataContext = randomGeneratorModel;

            foreach (RandomTableCategory randomTableCategory in randomGeneratorModel.ListOfTables.SubCategories)
            {
                randomTablesTree.Items.Add(randomTableCategory);
            }

        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            randomGeneratorModel.GenerateRandomTableSelection();
        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            randomGeneratorModel.RollDie();
        }

        private void randomTablesTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            randomGeneratorModel.CurrentTable = randomTablesTree.SelectedValue.ToString();
        }
    }
}
