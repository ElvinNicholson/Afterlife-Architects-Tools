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

namespace AfterlifeArchitectsTools
{
    /// <summary>
    /// Interaction logic for StructureEditor.xaml
    /// </summary>
    public partial class StructureEditor : Page
    {
        StructureType[] structureTypes = (StructureType[])Enum.GetValues(typeof(StructureType));

        public StructureEditor()
        {
            InitializeComponent();

            structureType_ComboBox.ItemsSource = structureTypes;
            structureType_ComboBox.SelectedValue = StructureType.Building_Green_T1;

            using (MagickImage image = new MagickImage("C:\\Users\\nicho\\Documents\\Scarle\\Afterlife-Architects-Tools\\AfterlifeArchitectsTools\\Building_Blue_Heaven_1x1.dds"))
            {
                byte[] imageData = image.ToByteArray(MagickFormat.Bmp);
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    var bitmapSource = System.Windows.Media.Imaging.BitmapFrame.Create(stream,
                        System.Windows.Media.Imaging.BitmapCreateOptions.None,
                        System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);

                    texturePreview.Source = bitmapSource;
                }
            }

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
            //MessageBox.Show(selectedType.ToString());
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
    }
}
