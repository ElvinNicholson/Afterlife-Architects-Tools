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

namespace AfterlifeArchitectsTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void structureEditor_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("StructureEditor.xaml", UriKind.Relative));
        }

        private void levelEditor_Click(Object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("LevelEditor.xaml", UriKind.Relative));
        }

        private void advisorEditor_Click(Object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("AdvisorEditor.xaml", UriKind.Relative));
        }
    }
}
