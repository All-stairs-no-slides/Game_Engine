using Game_Engine.faux_obj_types;
using Game_Engine.User_controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace Game_Engine
{
    /// <summary>
    /// unique object window for the object with its .obj file at obj_path
    /// </summary>
    public partial class ObjectViewWindow : Window
    {
        private string path;
        public Game_obj the_object;
        public double zoom;
        // is a tuple of the index of the place window, and the index of the Instance within said windows Instance variable it takes the form of [window pos, Instance array pos]
        int[] Instance_path;
        public ObjectViewWindow(string obj_path)
        {
            // for creating a window to edit the base object

            this.zoom = 0.5;
            this.path = obj_path;
            string jsonString = File.ReadAllText(obj_path);
            the_object = JsonConvert.DeserializeObject<Game_obj>(jsonString)!;
            
            InitializeComponent();

            Components_list.components = the_object.components;
            Components_list.Reload_components();
            Load_obj_visuals();

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
            Load_obj_visuals();
        }

        private void Load_obj_visuals()
        {
            foreach (game_component component in Components_list.components) 
            {
                // display a sprite
                if (component.GetType() == typeof(Sprite_renderer)) {
                    if(((Sprite_renderer)component).Sprite_dir == "") 
                    {
                        //location has yet to be assigned 4 this sprite
                        continue;
                    }
                    string json_string = File.ReadAllText(((Sprite_renderer)component).Sprite_dir);
                    Game_Sprite the_sprite = JsonConvert.DeserializeObject<Game_Sprite>(json_string);
                    Image_render_obj_display image_render_display = new Image_render_obj_display();

                    // Create Image Element
                    Image myImage = new Image();
                    //myImage.Width = 200;

                    // Create source
                    BitmapImage myBitmapImage = new BitmapImage();

                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    myBitmapImage.BeginInit();
                    myBitmapImage.UriSource = new Uri(the_sprite.Images_location[0]);

                    // To save significant application memory, set the DecodePixelWidth or
                    // DecodePixelHeight of the BitmapImage value of the image source to the desired
                    // height or width of the rendered image. If you don't do this, the application will
                    // cache the image as though it were rendered as its normal size rather than just
                    // the size that is displayed.
                    // Note: In order to preserve aspect ratio, set DecodePixelWidth
                    // or DecodePixelHeight but not both.
                    //myBitmapImage.DecodePixelWidth = 200;
                    myBitmapImage.EndInit();

                    image_render_display.image.Source = myBitmapImage;
                    image_render_display.sprite_viewbox.Width = myBitmapImage.Width * zoom;
                    image_render_display.sprite_viewbox.Height = myBitmapImage.Height * zoom;
                    Canvas.SetLeft(image_render_display, ((Sprite_renderer)component).x_offset);
                    Object_display.Children.Add(image_render_display);
                }
            }
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
                    TreeViewItem deletion_item = (TreeViewItem)Components_list.Tree_Parent.SelectedItem;
                    int deletion_index = (int)deletion_item.Tag;

                    the_object.components = the_object.components.Where((val, i) => i != deletion_index).ToArray();

                    Components_list.Tree_Parent.Items.Remove(Components_list.Tree_Parent.SelectedItem);
                    Components_list.Reload_components();
                    return;

                }
            }
        }
    }
}
