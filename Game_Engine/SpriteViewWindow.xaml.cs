using Game_Engine.faux_obj_types;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game_Engine
{
    /// <summary>
    /// Interaction logic for SpriteViewWindow.xaml
    /// </summary>
    public partial class SpriteViewWindow : Window
    {
        string path;
        Game_Sprite Sprite;
        public SpriteViewWindow(string Sprite_path)
        {
            this.path = Sprite_path;
            string json_string = File.ReadAllText(path);
            if (json_string != null)
            {
                Sprite = JsonConvert.DeserializeObject<Game_Sprite>(json_string);
            }
            //Debug.WriteLine(path);
            InitializeComponent();
        }

        private void Import_dir_click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.ShowDialog();

            if(openFolderDialog.FolderName != "")
            {
                string[] valid_types = { "png", "jpeg" };
                string[] files = Directory.GetFiles(openFolderDialog.FolderName);
                if (files.Length < 0) {
                    return;
                }
                foreach (string file in files) {
                    if (valid_types.Contains(file.Split(".").Last())){
                        if(Sprite.Images_location == null)
                        {
                            Sprite.Images_location = new List<string> { file };
                            Sprite.durations = new List<float> { (float)1 };
                            //Debug.WriteLine(file);

                        }
                        Sprite.Images_location.Add(file);
                        Sprite.durations.Add((float)1);
                    }
                }
            }
            
        }

        private void Save_Sprite()
        {
            // saving
            File.WriteAllText(path, JsonConvert.SerializeObject(Sprite));
        }
        private void Key_pressed(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S) { 
                Save_Sprite();
                return;
            }
        }
    }
}
