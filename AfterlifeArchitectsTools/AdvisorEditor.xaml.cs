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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ImageMagick;
using static System.Net.Mime.MediaTypeNames;

namespace AfterlifeArchitectsTools
{
    /// <summary>
    /// Interaction logic for AdvisorEditor.xaml
    /// </summary>
    public partial class AdvisorEditor : Page
    {
        private JObject advisorDialogues;
        private List<string> dialogues = new List<string>();
        private List<string> textboxes = new List<string>();
        private string[] standpoints = { "Heaven", "Hell", "Both", "Neither" };
        private string[] advisors = { "Jasper", "Aria" };
        private string filename;
        private string directory;

        private int selected_dialogue = 0;
        private int selected_textbox = 0;
        public AdvisorEditor()
        {
            InitializeComponent();
        }

        // --------- --------- --------- --------- --------- --------- --------- --------- ---------
        // SAVE/LOAD --------- --------- --------- --------- --------- --------- --------- ---------
        // --------- --------- --------- --------- --------- --------- --------- --------- ---------

        /// <summary>
        /// Advisor dialogue values JSON file loading
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

                loadAdvisorDialogues(filename);
            }
        }
        private void loadAdvisorDialogues(string path)
        {
            advisorDialogues = JObject.Parse(File.ReadAllText(path));
            dialogues.Clear();
            for (int i =0; i < (int)advisorDialogues["advisorDialogues"].Count(); i ++)
            {
                dialogues.Add(advisorDialogues["advisorDialogues"][i]["codeName"].ToString());
            }
            advisorDialogue_ComboBox.ItemsSource = dialogues;
            advisorTextbox_ComboBox.ItemsSource = new List<string>() { "Textbox 1", "Textbox 2" };
            advisorStandpoint_ComboBox.ItemsSource = standpoints;
            advisorSpeaking_ComboBox.ItemsSource = advisors;
        }

        private void advisorSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("There is no loaded AdvisorDialogues.json");
                return;
            }

            File.WriteAllText(filename, advisorDialogues.ToString(Formatting.Indented));
        }

        // --------- --------- --------- --------- --------- --------- --------- --------- ---------
        // COMBOBOXES--------- --------- --------- --------- --------- --------- --------- ---------
        // --------- --------- --------- --------- --------- --------- --------- --------- ---------

        private void advisorDialogue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (advisorDialogue_ComboBox.SelectedIndex == -1)
            {
                return;
            }
            selected_dialogue = advisorDialogue_ComboBox.SelectedIndex;
            selected_textbox = 0;

            // Textboxes
            codename.Text = dialogues[selected_dialogue];
            title.Text = advisorDialogues["advisorDialogues"][selected_dialogue]["dialogueTitle"].ToString();
            textbox.Text = advisorDialogues["advisorDialogues"][selected_dialogue]["texts"][0].ToString();

            // Combo boxes
            advisorStandpoint_ComboBox.SelectedIndex = (int)advisorDialogues["advisorDialogues"][selected_dialogue]["standpoint"];
            advisorTextbox_ComboBox.SelectedIndex = 0;
            advisorSpeaking_ComboBox.SelectedIndex = (int)advisorDialogues["advisorDialogues"][selected_dialogue]["whosTalking"][0];
            List<string> temp_list = new List<string>();
            for (int i = 0; i < advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Count(); i++)
            {
                temp_list.Add("Textbox " + (i + 1).ToString());
            }
            advisorTextbox_ComboBox.ItemsSource = temp_list;
        }

        private void advisorStandpoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            advisorDialogues["advisorDialogues"][selected_dialogue]["standpoint"] = advisorStandpoint_ComboBox.SelectedIndex;
        }

        private void advisorTextbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (advisorTextbox_ComboBox.SelectedIndex == -1)
            {
                return;
            }
            selected_textbox = advisorTextbox_ComboBox.SelectedIndex;

            // Combo boxes
            List<string> temp_list = new List<string>();
            for (int i = 0; i < advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Count(); i++)
            {
                temp_list.Add("Textbox "+(i+1).ToString());
            }
            advisorTextbox_ComboBox.ItemsSource = temp_list;
            advisorSpeaking_ComboBox.SelectedIndex = (int)advisorDialogues["advisorDialogues"][selected_dialogue]["whosTalking"][selected_textbox];

            // Texts
            textbox.Text = advisorDialogues["advisorDialogues"][selected_dialogue]["texts"][selected_textbox].ToString();
        }

        private void advisorSpeaking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            advisorDialogues["advisorDialogues"][selected_dialogue]["whosTalking"][selected_textbox] = advisorSpeaking_ComboBox.SelectedIndex;
        }

        // --------- --------- --------- --------- --------- --------- --------- --------- ---------
        // BUTTONS   --------- --------- --------- --------- --------- --------- --------- ---------
        // --------- --------- --------- --------- --------- --------- --------- --------- ---------

        private void advisorAddDialogue_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            // Add new instance of dialogue
            JObject newDialogue = new JObject();
            newDialogue.Add("dialogueTitle", "New Title");
            newDialogue.Add("codeName", "New Codename");
            newDialogue.Add("standpoint", 0);
            JArray texts = new JArray();
            texts.Add("New text 1");
            texts.Add("New text 2");
            newDialogue.Add("texts", texts);
            newDialogue.Add("whosTalking", "01");
            advisorDialogues["advisorDialogues"].Last.AddAfterSelf(newDialogue);

            // Update combo boxes
            List<string> temp_list = new List<string>();
            for (int i = 0; i < dialogues.Count(); i ++)
            {
                temp_list.Add(dialogues[i]);
            }
            temp_list.Add("New Codename");
            dialogues = temp_list;
            advisorDialogue_ComboBox.ItemsSource = dialogues;
            advisorDialogue_ComboBox.SelectedIndex = advisorDialogues["advisorDialogues"].Count() - 1;

        }

        private void advisorRemoveDialogue_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename) || advisorDialogues["advisorDialogues"].Count() <= 1)
            {
                return;
            }

            // Remove selected instance of dialogue
            advisorDialogues["advisorDialogues"][selected_dialogue].Remove();

            // Update combo boxes
            List<string> temp_list = new List<string>();
            for (int i = 0; i < dialogues.Count(); i++)
            {
                if (i != selected_dialogue)
                {
                    temp_list.Add(dialogues[i]);
                }
            }
            dialogues = temp_list;
            advisorDialogue_ComboBox.ItemsSource = dialogues;
            selected_dialogue = advisorDialogues["advisorDialogues"].Count() - 1;
            advisorDialogue_ComboBox.SelectedIndex = selected_dialogue;
        }

        private void advisorAddTextbox_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Last.AddAfterSelf("New textbox started");
            advisorDialogues["advisorDialogues"][selected_dialogue]["whosTalking"].Last.AddAfterSelf(0);
            advisorTextbox_ComboBox.SelectedIndex = advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Count()-1;
            // Update combo boxes + texts
            List<string> temp_list = new List<string>();
            for (int i = 0; i < advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Count(); i++)
            {
                temp_list.Add("Textbox " + (i + 1).ToString());
            }
            advisorTextbox_ComboBox.ItemsSource = temp_list;
        }

        private void advisorRemoveTextbox_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename) || advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Count() == 1)
            {
                return;
            }
            advisorDialogues["advisorDialogues"][selected_dialogue]["texts"][selected_textbox].Remove();
            advisorDialogues["advisorDialogues"][selected_dialogue]["whosTalking"][selected_textbox].Remove();
            selected_textbox = advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Count() - 1;
            advisorTextbox_ComboBox.SelectedIndex = selected_textbox;
            // Update combo boxes + texts
            List<string> temp_list = new List<string>();
            for (int i = 0; i < advisorDialogues["advisorDialogues"][selected_dialogue]["texts"].Count(); i++)
            {
                temp_list.Add("Textbox " + (i + 1).ToString());
            }
            advisorTextbox_ComboBox.ItemsSource = temp_list;
        }

        // --------- --------- --------- --------- --------- --------- --------- --------- ---------
        // TEXTBOXES --------- --------- --------- --------- --------- --------- --------- ---------
        // --------- --------- --------- --------- --------- --------- --------- --------- ---------

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            advisorDialogues["advisorDialogues"][selected_dialogue]["dialogueTitle"] = title.Text;
        }

        private void codename_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            advisorDialogues["advisorDialogues"][selected_dialogue]["codeName"] = codename.Text;
            dialogues[selected_dialogue] = codename.Text;

            List<string> temp_list = new List<string>();
            for (int i = 0; i < dialogues.Count(); i++)
            {
                if (i == selected_dialogue)
                {
                    temp_list.Add(codename.Text);
                }
                else
                {
                    temp_list.Add(dialogues[i]);
                }
            }
            dialogues = temp_list;
            advisorDialogue_ComboBox.ItemsSource = dialogues;
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            advisorDialogues["advisorDialogues"][selected_dialogue]["texts"][selected_textbox] = textbox.Text;
        }

        // --------- --------- --------- --------- --------- --------- --------- --------- ---------
        // PREVIEWS  --------- --------- --------- --------- --------- --------- --------- ---------
        // --------- --------- --------- --------- --------- --------- --------- --------- ---------

        private void title_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void codename_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }

}
