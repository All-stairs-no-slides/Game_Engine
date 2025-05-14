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
        public ObjectViewWindow(string obj_path)
        {
            this.path = obj_path;
            string jsonString = File.ReadAllText(obj_path);
            the_object = JsonConvert.DeserializeObject<Game_obj>(jsonString)!;
            
            InitializeComponent();

            Components_list.components = the_object.components;
            Components_list.Reload_components();

        }
        private void Save_Object()
        {
            string json_string = JsonConvert.SerializeObject(the_object);
            File.WriteAllText(path, json_string);
            
        }
        private void Key_pressed(object sender, KeyEventArgs e)
        {
            // check for save kepress
            if(Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Save_Object();
            }
        }
    }
}
