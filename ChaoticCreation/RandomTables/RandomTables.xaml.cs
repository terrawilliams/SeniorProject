using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using ChaoticCreation.RandomTables;

namespace ChaoticCreation.RandomTables
{
    /// <summary>
    /// Interaction logic for RandomTables.xaml
    /// </summary>
    public partial class RandomTables : UserControl
    {
        private TreeView RANDOM_TABLES_GLOBAL = new TreeView();
        List<RandomTableCategory> table = new List<RandomTableCategory>();
        List<RandomTableCategory> x = new List<RandomTableCategory>();
        bool firstInitialize = false;

        RandomGeneratorModel randomGeneratorModel = new RandomGeneratorModel();

        public RandomTables()
        {
            InitializeComponent();
            this.DataContext = randomGeneratorModel;

            foreach(RandomTableCategory randomTableCategory in randomGeneratorModel.ListOfTables.SubCategories)
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
            try
            {
                var table = TableList.ItemsSource as IList<KeyValuePair<string, string>>;

                if (table.Count != 0)
                {
                    DiceSize.Text = table.Count.ToString();

                }

                int result = Int32.Parse(DiceSize.Text);

                result = randomGeneratorModel.RollDie(result);
                dieFace.Text = result.ToString();

                try
                {

                    if ((result % table.Count) == 0)
                    {
                        result = table.Count - 1;
                    }
                    else
                    {
                        result = result % table.Count - 1;
                    }
                    for (int i = 0; i <= result; i++)
                    {
                        if (i == result)
                        {
                            Console.WriteLine(table[i].Value);
                            TableList.SelectedItem = table[i];
                        }
                    }

                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Divide by Zero (This is caused by not selecting a table in random table, so generally okay)");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Move Next failed (line 52 RandomTables.xaml.cs)");
                }
            }
            catch (FormatException)
            {
                dieFace.Text = "Error";
            }
        }

        private void randomTablesTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            randomGeneratorModel.CurrentTable = randomTablesTree.SelectedValue.ToString();
        }

        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if ((table.Count == 0) && (!firstInitialize))
            {
                for (int i = 0; i < randomTablesTree.Items.Count; i++)
                {
                    table.Add((RandomTableCategory)randomTablesTree.Items.GetItemAt(i));
                }
                firstInitialize = true;
                randomTablesTree.Items.Clear();
                randomTablesTree.ItemsSource = table;
                for (int i = 0; i < table.Count; i++)
                {
                    x.Add((RandomTableCategory)table[i]);
                }
            }
            else
            {
                randomTablesTree.ItemsSource = null;
                table.Clear();
                for (int i = 0; i < x.Count; i++)
                {
                    table.Add((RandomTableCategory)x[i]);
                }
                randomTablesTree.ItemsSource = table;
                return;
            }
            List<RandomTableCategory> newTree = new List<RandomTableCategory>();
            for (int i = 0; i < table.Count; i++)
            {
                var y = table[i];
                int k = ((RandomTableCategory)y).SubCategories.Count;

                if (k == 0)
                {
                    if (((RandomTableCategory)y).Name.Contains(SearchBox.Text))
                    {
                        Console.WriteLine(((RandomTableCategory)y).Name);
                        newTree.Add((RandomTableCategory)y);
                    }
                }

                for (int j = 0; j < k; j++)
                {
                    if (((RandomTableCategory)y).SubCategories[j].Name.Contains(SearchBox.Text))
                    {
                        Console.WriteLine(((RandomTableCategory)y).SubCategories[j].Name);
                        newTree.Add(((RandomTableCategory)y).SubCategories[j]);
                    }
                }
                
            }
            randomTablesTree.ItemsSource = null;
            table.Clear();
            for (int i = 0; i < newTree.Count; i++)
            {
                table.Add(newTree[i]);
            }
            randomTablesTree.ItemsSource = table;
        }

    }

}
