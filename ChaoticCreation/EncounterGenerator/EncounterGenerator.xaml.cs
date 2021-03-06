﻿using System;
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

namespace ChaoticCreation.EncounterGenerator
{
    /// <summary>
    /// Interaction logic for EncounterGenerator.xaml
    /// </summary>
    public partial class EncounterGenerator : UserControl
    {
        EncounterGeneratorModel encounterGeneratorModel = new EncounterGeneratorModel();
        public EncounterGenerator()
        {
            InitializeComponent();
            this.DataContext = encounterGeneratorModel;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            encounterGeneratorModel.GenerateEncounter();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            encounterGeneratorModel.SaveCurrentEnocunter();
        }
    }
}
