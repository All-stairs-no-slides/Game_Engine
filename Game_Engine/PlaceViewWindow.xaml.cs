using System;
using System.IO;
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
using System.Windows.Shapes;
using Game_Engine.faux_obj_types;
using Newtonsoft.Json;

namespace Game_Engine
{
    /// <summary>
    /// Interaction logic for PlaceViewWindow.xaml
    /// </summary>
    public partial class PlaceViewWindow : Window
    {
        public Game_Place Place;
        public string path;
        public PlaceViewWindow(string place_path)
        {
            this.path = place_path;
            string jsonString = File.ReadAllText(place_path);
            Place = JsonConvert.DeserializeObject<Game_Place>(jsonString)!;
            InitializeComponent();

            // add all of the Instances to the list on startup of window
            int index = 0;
            foreach(Game_obj inst in Place.Instances)
            {
                Add_Instance_to_Visual_List(inst, index);
                index++;
            }
        }

        private Game_obj Get_Instance(string Obj_path)
        {
            // for getting an object instance from a path to its json file
            string jsonString = File.ReadAllText(Obj_path);
            return JsonConvert.DeserializeObject<Game_obj>(jsonString)!;
        }

        private void Add_Instance_to_Visual_List(Game_obj inst, int index)
        {
            // summary:
            // adds the visual indicator of the instance to the Place windows instance list
            TreeViewItem Instance_List_Item = new TreeViewItem();
            Instance_List_Item.Header = inst.Name;
            Instance_List_Item.Tag = index;
            //Debug.WriteLine(Instance_List_Item.Tag.GetType());
            Instance_List_Item.MouseDoubleClick += Open_Instance_Window;
            Instance_list.Tree_Parent.Items.Add(Instance_List_Item);
        }

        private void Place_Window_Drop(object sender, DragEventArgs e)
        {
            // creating a instance of  an object when it is dragged into the Place window
            if (((string)e.Data.GetData(DataFormats.StringFormat)).Split("\\").Last().Split(".").Last() == "obj")
            {
                Game_obj Instance = Get_Instance((string)e.Data.GetData(DataFormats.StringFormat));
                if (Place.Instances != null)
                {
                    Place.Instances = Place.Instances.Append(Instance).ToArray();
                }
                else
                {
                    Place.Instances = [Instance];
                }
                
                Add_Instance_to_Visual_List(Instance, Place.Instances.Length - 1);


            }
        }

        private void Open_Instance_Window(object sender, MouseButtonEventArgs e)
        {
            int window_index = 0;
            foreach (Window window in Application.Current.Windows.OfType<PlaceViewWindow>())
            {
                window_index++;
                if (window == this)
                {
                    TreeViewItem source_item = e.Source as TreeViewItem;
                    ObjectViewWindow Instance_window = new ObjectViewWindow(Place.Instances[(int)source_item.Tag], (int)source_item.Tag, window_index);
                    Instance_window.Show();
                    break;
                }
            }

        }

        private void Save_Place()
        {
            string json_string = JsonConvert.SerializeObject(Place);
            File.WriteAllText(path, json_string);

        }
        private void Key_pressed(object sender, KeyEventArgs e)
        {
            // check for save kepress
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Save_Place();
            }
            else if (e.Key == Key.Delete)
            {
                if (Instance_list.Tree_Parent.SelectedItem != null)
                {
                    TreeViewItem deletion_item = (TreeViewItem)Instance_list.Tree_Parent.SelectedItem;
                    int deletion_index = (int)deletion_item.Tag;
                    Place.Instances = Place.Instances.Where((val, i) => i != deletion_index).ToArray();
                    Instance_list.Tree_Parent.Items.Remove(Instance_list.Tree_Parent.SelectedItem);
                }
            }
        }
    }
}
