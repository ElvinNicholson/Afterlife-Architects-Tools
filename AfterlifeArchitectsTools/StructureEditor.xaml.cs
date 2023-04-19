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
using static System.Net.Mime.MediaTypeNames;

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

        private StructureType selectedType;
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
        private void regex_number(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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

        /// <summary>
        /// Changes the displayed data according to the structureType comboBox
        /// </summary>
        /// <param name="type">StructureType to be shown</param>
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

            // Show/Hide panels
            soulCapacity_DockPanel.Visibility = Visibility.Collapsed;
            soulRate_DockPanel.Visibility = Visibility.Collapsed;
            ADCapacity_DockPanel.Visibility = Visibility.Collapsed;
            conversionRate_DockPanel.Visibility = Visibility.Collapsed;

            switch (type)
            {
                case StructureType.Building_Green_T1:
                case StructureType.Building_Green_T2:
                case StructureType.Building_Yellow_T1:
                case StructureType.Building_Yellow_T2:
                case StructureType.Building_Orange_T1:
                case StructureType.Building_Orange_T2:
                case StructureType.Building_Brown_T1:
                case StructureType.Building_Brown_T2:
                case StructureType.Building_Purple_T1:
                case StructureType.Building_Purple_T2:
                case StructureType.Building_Red_T1:
                case StructureType.Building_Red_T2:
                case StructureType.Building_Blue_T1:
                case StructureType.Building_Blue_T2:
                    soulCapacity_DockPanel.Visibility = Visibility.Visible;
                    soulCapacity.Text = (string)structureValue["structureValues"][(int)type]["soulCapacity"];
                    break;

                case StructureType.Gate_T1:
                case StructureType.Gate_T2:
                case StructureType.Gate_T3:
                    soulRate_DockPanel.Visibility = Visibility.Visible;
                    soulRate.Text = (string)structureValue["structureValues"][(int)type]["soulRate"];
                    break;

                case StructureType.Topia_T1:
                case StructureType.Topia_T2:
                    ADCapacity_DockPanel.Visibility = Visibility.Visible;
                    ADCapacity.Text = (string)structureValue["structureValues"][(int)type]["ADCapacity"];
                    break;

                case StructureType.TrainingCenter_T1:
                case StructureType.TrainingCenter_T2:
                case StructureType.TrainingCenter_T3:
                    conversionRate_DockPanel.Visibility = Visibility.Visible;
                    conversionRate.Text = (string)structureValue["structureValues"][(int)type]["conversionRate"];
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Structure values JSON file saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structureSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            File.WriteAllText(filename, structureValue.ToString(Formatting.Indented));
        }

        /// <summary>
        /// Triggers when user changes the selected Structure Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structureType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedType = (StructureType)(structureType_ComboBox.SelectedItem);

            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            displayStructure(selectedType);
        }

        /// <summary>
        /// Saves the changes to cost to the JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cost_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            structureValue["structureValues"][(int)selectedType]["cost"] = int.Parse(cost.Text);
        }

        /// <summary>
        /// Saves the changes to tile size to the JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void size_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            structureValue["structureValues"][(int)selectedType]["tileSize"] = int.Parse(size.Text);
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
                    RenderOptions.SetBitmapScalingMode(texturePreview, BitmapScalingMode.NearestNeighbor);
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
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DDS image (*.dds)|*.dds";

            openFileDialog.InitialDirectory = System.IO.Path.Combine(directory, "Assets");
            if (openFileDialog.ShowDialog() == true)
            {
                textureHeaven = openFileDialog.FileName;
                textureHeaven_TextBlock.Text = openFileDialog.SafeFileName;

                string filename = openFileDialog.SafeFileName;
                filename = filename.Substring(0, filename.Length - 4);
                structureValue["structureValues"][(int)selectedType]["textureHeaven"] = filename;

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
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DDS image (*.dds)|*.dds";

            openFileDialog.InitialDirectory = System.IO.Path.Combine(directory, "Assets");
            if (openFileDialog.ShowDialog() == true)
            {
                textureHell = openFileDialog.FileName;
                textureHell_TextBlock.Text = openFileDialog.SafeFileName;

                string filename = openFileDialog.SafeFileName;
                filename = filename.Substring(0, filename.Length - 4);
                structureValue["structureValues"][(int)selectedType]["textureHell"] = filename;

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
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            loadImage(textureHeaven);
        }

        /// <summary>
        /// Changes the texturePreview to Hell texture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewHell_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            loadImage(textureHell);
        }

        private void soulCapacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            structureValue["structureValues"][(int)selectedType]["soulCapacity"] = int.Parse(soulCapacity.Text);
        }

        private void soulRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            structureValue["structureValues"][(int)selectedType]["soulRate"] = int.Parse(soulRate.Text);
        }

        private void ADCapacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            structureValue["structureValues"][(int)selectedType]["ADCapacity"] = int.Parse(ADCapacity.Text);
        }

        private void conversionRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded StructureValues.json");
                return;
            }

            structureValue["structureValues"][(int)selectedType]["conversionRate"] = int.Parse(conversionRate.Text);
        }
    }
}
