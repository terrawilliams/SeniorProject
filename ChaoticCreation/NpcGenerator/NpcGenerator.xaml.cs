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

namespace ChaoticCreation.NpcGenerator
{
    /// <summary>
    /// Interaction logic for NpcGenerator.xaml
    /// </summary>
    public partial class NpcGenerator : UserControl
    {
        NpcGeneratorModel npcGeneratorModel = new NpcGeneratorModel();
        public NpcGenerator()
        {
            InitializeComponent();
            this.DataContext = npcGeneratorModel;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            npcGeneratorModel.GenerateNewNpc();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            npcGeneratorModel.SaveCurrentNpc();
        }
    }
}
