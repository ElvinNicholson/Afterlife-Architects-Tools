using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        }

        private void structureLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void structureSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void structureType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StructureType selectedType = (StructureType)(structureType_ComboBox.SelectedItem);
            //MessageBox.Show(selectedType.ToString());
        }
    }
}
