using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using Game_Engine.User_controls;

namespace Game_Engine
{
    /// <summary>
    /// unique object window for the object with its .obj file at obj_path
    /// </summary>
    public partial class ObjectViewWindow : Window
    {
        private string path;
        private FileStream obj_settings;
        public ObjectViewWindow(string obj_path)
        {
            obj_settings = File.Open(obj_path, FileMode.Open);

            path = obj_path.Remove(obj_path.LastIndexOf("\\"));
            Debug.WriteLine(path);
            InitializeComponent();
            Components_list.Path = obj_path;

        }
    }
}
