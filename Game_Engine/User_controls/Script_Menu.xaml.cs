using Game_Engine.faux_obj_types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Script_Menu.xaml
    /// </summary>
    public partial class Script_Menu : UserControl
    {
        public Script_Menu(int index, string path, string scope)
        {
            this.Index = index;
            this.Path_prop = path;
            this.Scope_prop = scope;
            InitializeComponent();
        }

        private void Script_selection(object sender, RoutedEventArgs e)
        {

        }

        private void scope_changed(object sender, SelectionChangedEventArgs e)
        {
            Scope_prop = (string)((ComboBoxItem)((ComboBox)sender).SelectedItem).Content;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private int Index;

        private string path;

        public string Path_prop
        {
            get { return path; }
            set
            {
                path = value;
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
                                ((script_component)comps[Index]).path = value;
                            }
                        }
                    }

                }
            }
        }

        private string scope;

        public string Scope_prop
        {
            get { return scope; }
            set
            {
                path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Scope_prop"));
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
                                ((script_component)comps[Index]).scope = value;
                            }
                        }
                    }

                }
            }
        }

        
    }
}
