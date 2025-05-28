using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
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
using Game_Engine.User_controls;

namespace Game_Engine
{
    /// <summary>
    /// unique object window for the object with its .obj file at obj_path
    /// </summary>
    public partial class ObjectViewWindow : Window
    {
        private string path;
        public Game_obj the_object;
        // is a tuple of the index of the place window, and the index of the Instance within said windows Instance variable it takes the form of [window pos, Instance array pos]
        int[] Instance_path;
        public ObjectViewWindow(string obj_path)
        {
            // for creating a window to edit the base object


            this.path = obj_path;
            string jsonString = File.ReadAllText(obj_path);
            the_object = JsonConvert.DeserializeObject<Game_obj>(jsonString)!;
            
            InitializeComponent();

            Components_list.components = the_object.components;
            Components_list.Reload_components();

        }
        public ObjectViewWindow(Game_obj Instance, int Instance_arr_pos, int Place_window_index)
        {
            // for creating a window from a Place's instance to edit for said place

            Instance_path = [Place_window_index, Instance_arr_pos];

            path = "";
            the_object = Instance;
            InitializeComponent();
            Components_list.components = the_object.components;
            Components_list.Reload_components();
        }
        private void Save_Object()
        {
            if (Instance_path == null)
            {
                string json_string = JsonConvert.SerializeObject(the_object);
                File.WriteAllText(path, json_string);
                return;
            } else
            {
                // for the case of the window being about an instance
                // finds the place window of instance, finds instance, replaces instance
                int index = 0;
                foreach(PlaceViewWindow window in Application.Current.Windows.OfType<PlaceViewWindow>())
                {
                    if(index == Instance_path[0])
                    {
                        window.Place.Instances[Instance_path[1]] = the_object;
                        return;
                    }
                    index++;
                }
                
            }
        }
        private void Key_pressed(object sender, KeyEventArgs e)
        {
            // check for save kepress
            if(Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Save_Object();
            } else if (e.Key == Key.Delete)
            {
                if(Components_list.Tree_Parent.SelectedItem != null)
                {
                    return;
                    the_object.components = the_object.components.Where((val, i) => );

                    Components_list.Tree_Parent.Items.Remove(Components_list.Tree_Parent.SelectedItem);
                }
            }
        }
    }
}
