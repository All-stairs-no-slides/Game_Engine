using Game_Engine.faux_obj_types;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Audio_Menu.xaml
    /// </summary>
    public partial class Audio_Menu : UserControl
    {
        public Audio_Menu(int index, string path, string alias)
        {
            this.Index = index;
            this.Path_prop = path;
            this.Alias_prop = alias;
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // index is used to find this component within the objects components array
        private int Index;

        private string Path;

        public string Path_prop
        {
            get { return Path; }
            set
            {
                Path = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Path_prop"));
                //find window that this is in to update the object component list for
                foreach (ObjectViewWindow window in Application.Current.Windows.OfType<ObjectViewWindow>())
                {
                    //Debug.WriteLine(((ObjectViewWindow)window).Components_list.Tree_Parent.Items);
                    foreach (TreeViewItem item in ((ObjectViewWindow)window).Components_list.Tree_Parent.Items)
                    {
                        foreach (object content in item.Items)
                        {
                            if (content == this)
                            {
                                game_component[] comps = window.the_object.components;
                                ((audio_component)comps[Index]).path = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

        private string Alias;

        public string Alias_prop
        {
            get { return Alias; }
            set
            {
                Alias = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Alias_prop"));
                //find window that this is in to update the object component list for
                foreach (ObjectViewWindow window in Application.Current.Windows.OfType<ObjectViewWindow>())
                {
                    //Debug.WriteLine(((ObjectViewWindow)window).Components_list.Tree_Parent.Items);
                    foreach (TreeViewItem item in ((ObjectViewWindow)window).Components_list.Tree_Parent.Items)
                    {
                        foreach (object content in item.Items)
                        {
                            if (content == this)
                            {
                                game_component[] comps = window.the_object.components;
                                ((audio_component)comps[Index]).sound_alias = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

        private void Audio_selection(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio File|*.wav";
            openFileDialog.Title = "Choose an Audio File";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)openFileDialog.OpenFile();
                string[] segmented = fs.Name.Split("\\");
                this.Path_prop = segmented.Last();
            }
        }
    }
}
