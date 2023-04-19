using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AfterlifeArchitectsTools
{
    /// <summary>
    /// Interaction logic for AdvisorEditor.xaml
    /// </summary>
    public partial class AdvisorEditor : Page
    {
        private string filename;
        private string directory;
        public AdvisorEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Structure values JSON file loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advisorLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "JSON files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;

                // Get root folder of game
                directory = System.IO.Path.GetDirectoryName(filename);
                directory = System.IO.Path.GetDirectoryName(directory);
                directory = System.IO.Path.GetDirectoryName(directory);
            }
        }

        private void advisorSave_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
        }

        private void advisorDialogue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void advisorAddDialogue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void advisorRemoveDialogue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void advisorStandpoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void advisorTextbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void advisorSpeaking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void title_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
        
        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void advisorAddTextbox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void advisorRemoveTextbox_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
