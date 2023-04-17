using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using ImageMagick;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AfterlifeArchitectsTools
{
    /// <summary>
    /// Interaction logic for StructureEditor.xaml
    /// </summary>
    public partial class StructureEditor : Page
    {
        StructureType[] structureTypes = (StructureType[])Enum.GetValues(typeof(StructureType));

        private JObject structureValue;
        private string filename;
        private string directory;

        private string textureHeaven;
        private string textureHell;

        public StructureEditor()
        {
            InitializeComponent();

            structureType_ComboBox.ItemsSource = structureTypes;
        }

        /// <summary>
        /// Regex that only allow numbers
        /// </summary>
        /// <param name="text">The string component of the TextBox</param>
        /// <returns></returns>
        private bool regexNumber(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(text);
        }

        /// <summary>
        /// Structure values JSON file loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structureLoad_Click(object sender, RoutedEventArgs e)
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

                loadStructureValue(filename);
            }
        }

        private void loadStructureValue(string path)
        {
            structureValue = JObject.Parse(File.ReadAllText(path));
        }

        private void displayStructure(StructureType type)
        {
            cost.Text = (string)structureValue["structureValues"][(int)type]["cost"];
            size.Text = (string)structureValue["structureValues"][(int)type]["tileSize"];

            textureHeaven_TextBlock.Text = (string)structureValue["structureValues"][(int)type]["textureHeaven"] + ".dds";
            textureHell_TextBlock.Text = (string)structureValue["structureValues"][(int)type]["textureHell"] + ".dds";

            string imagePath = System.IO.Path.Combine(directory, "Assets");

            textureHeaven = System.IO.Path.Combine(imagePath, textureHeaven_TextBlock.Text);
            textureHell = System.IO.Path.Combine(imagePath, textureHell_TextBlock.Text);

            loadImage(textureHeaven);
        }

        /// <summary>
        /// Structure values JSON file saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structureSave_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Triggers when user changes the selected Structure Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structureType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StructureType selectedType = (StructureType)(structureType_ComboBox.SelectedItem);
            displayStructure(selectedType);
        }

        /// <summary>
        /// Saves the changes to cost to the JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cost_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Only allow numbers to be input to the cost field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regexNumber(e.Text);
        }

        /// <summary>
        /// Saves the changes to tile size to the JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void size_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Only allow numbers to be input to the tile size field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regexNumber(e.Text);
        }

        /// <summary>
        /// Loads DDS texture to texturePreview
        /// </summary>
        /// <param name="path">Path of the DDS image</param>
        private void loadImage(string path)
        {
            using (MagickImage image = new MagickImage(path))
            {
                byte[] imageData = image.ToByteArray(MagickFormat.Bmp);

                using (var stream = new MemoryStream(imageData))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    texturePreview.Source = bitmapImage;
                    texturePreview.Width = 250;
                    texturePreview.Height = 250;
                }
            }
        }

        /// <summary>
        /// Opens a file dialogue to change the heaven texture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textureHeavenLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DDS image (*.dds)|*.dds";

            openFileDialog.InitialDirectory = System.IO.Path.Combine(directory, "Assets");
            if (openFileDialog.ShowDialog() == true)
            {
                textureHeaven = openFileDialog.FileName;
                textureHeaven_TextBlock.Text = openFileDialog.SafeFileName;
                loadImage(textureHeaven);
            }
        }

        /// <summary>
        /// Opens a file dialogue to change the hell texture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textureHellLoad_Click(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DDS image (*.dds)|*.dds";

            openFileDialog.InitialDirectory = System.IO.Path.Combine(directory, "Assets");
            if (openFileDialog.ShowDialog() == true)
            {
                textureHell = openFileDialog.FileName;
                textureHell_TextBlock.Text = openFileDialog.SafeFileName;
                loadImage(textureHell);
            }
        }

        /// <summary>
        /// Changes the texturePreview to heaven texture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewHeaven_Click(object sender, RoutedEventArgs e)
        {
            loadImage(textureHeaven);
        }

        /// <summary>
        /// Changes the texturePreview to Hell texture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewHell_Click(object sender, RoutedEventArgs e)
        {
            loadImage(textureHell);
        }
    }
}
