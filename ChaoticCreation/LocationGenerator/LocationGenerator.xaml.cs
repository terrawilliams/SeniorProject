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

namespace ChaoticCreation.LocationGenerator
{
    /// <summary>
    /// Interaction logic for LocationGenerator.xaml
    /// </summary>
    public partial class LocationGenerator : UserControl
    {
        LocationGeneratorModel locationGeneratorModel = new LocationGeneratorModel();
        public LocationGenerator()
        {
            InitializeComponent();
            this.DataContext = locationGeneratorModel;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            locationGeneratorModel.GenerateLocation();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            locationGeneratorModel.SaveLocation();
        }
    }
}
