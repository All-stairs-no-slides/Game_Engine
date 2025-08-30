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
using System.Windows.Controls.Primitives;
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

            this.Loaded += Load_obj_visuals;

        }
        public ObjectViewWindow(Game_obj Instance, int Instance_arr_pos, int Place_window_index)
        {
            // for creating a window from a Place's instance to edit for said place

            Instance_path = [Place_window_index, Instance_arr_pos];
            this.zoom = 0.5;
            path = "";
            the_object = Instance;
            InitializeComponent();
            Components_list.components = the_object.components;
            Components_list.Reload_components();
            this.Loaded += Load_obj_visuals;
        }

        private void Load_obj_visuals(object sender, RoutedEventArgs e)
        {
            // for converting from piels to dots per inch
            PresentationSource psource = PresentationSource.FromVisual(Object_display);
            double x_dpi_scale;
            double y_dpi_scale;
            if (psource != null)
            {
                Matrix dpi_trans = psource.CompositionTarget.TransformToDevice;
                x_dpi_scale = dpi_trans.M11;
                y_dpi_scale = dpi_trans.M22;
            }
            else {
                Debug.Write("an issue with dpi source (not found)");
                return;
            }

            // the position in the components array so each element can easily be refered to
            int index = 0;
            foreach (game_component component in Components_list.components) 
            {
                // display a sprite
                if (component.GetType() == typeof(Sprite_renderer)) {
                    if(((Sprite_renderer)component).Sprite_dir == "") 
                    {
                        //location has yet to be assigned 4 this sprite
                        index++;
                        continue;
                    }
                    string json_string = File.ReadAllText(((Sprite_renderer)component).Sprite_dir);
                    Game_Sprite the_sprite = JsonConvert.DeserializeObject<Game_Sprite>(json_string);

                    if (the_sprite.Images_location.Count == 0) {
                        index++;
                        continue;
                    }


                    Image_render_obj_display image_render_display = new Image_render_obj_display();

                    // Create source
                    BitmapImage myBitmapImage = new BitmapImage();

                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    myBitmapImage.BeginInit();
                    myBitmapImage.UriSource = new Uri(the_sprite.Images_location[0]);
                    myBitmapImage.EndInit();

                    

                    image_render_display.image.Source = myBitmapImage;
                    image_render_display.sprite_viewbox.Width = ((myBitmapImage.PixelWidth / x_dpi_scale) * zoom) * ((Sprite_renderer)component).x_scale;
                    image_render_display.sprite_viewbox.Height = ((myBitmapImage.PixelHeight / y_dpi_scale) * zoom) * ((Sprite_renderer)component).y_scale;
                    Canvas.SetLeft(image_render_display, ((((Sprite_renderer)component).x_offset / x_dpi_scale) * zoom));
                    Canvas.SetTop(image_render_display, ((((Sprite_renderer)component).y_offset / y_dpi_scale) * zoom));
                    Canvas.SetZIndex(image_render_display, ((Sprite_renderer)component).depth);

                    image_render_display.Tag = index;
                    Object_display.Children.Add(image_render_display);
                }
                index++;

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

        private void Expanding_image(object[] data, Point move_to)
        {
            //Debug.WriteLine(sender.GetType());
            //Canvas.GetLeft(sender);
            if (data[1] is not Image_render_obj_display || data[0] is not string)
            {
                Debug.WriteLine("invalid selection entered image_scaling");
                return;
            }

            string expand_type = (string)data[0];
            //state just before this one
            Image_render_obj_display the_image_obj = (Image_render_obj_display)data[1];


            //Debug.WriteLine(e.Source);

            foreach (UIElement child in Object_display.Children)
            {
                if (child == data[1])
                {
                    // this is the prior state of the image before the stretching
                    Sprite_renderer element = (Sprite_renderer)Components_list.components[(int)the_image_obj.Tag];
                    

                    switch (expand_type)
                    {
                        case "SW_Thumb":


                            // check that you arent inverting the image (with a negative height)
                            if (move_to.Y - element.y_offset < 0)
                            {
                                break;
                            }

                            // check that you arent inverting the image (with a negative width)
                            if (((((Image_render_obj_display)child).sprite_viewbox.Width) + element.x_offset) - move_to.X < 0)
                            {
                                break;
                            }

                            Canvas.SetLeft(child, move_to.X);

                            ((Image_render_obj_display)child).Height = move_to.Y - element.y_offset;
                            ((Image_render_obj_display)child).Width = ((((Image_render_obj_display)child).sprite_viewbox.Width) + element.x_offset) - move_to.X;

                            break;

                        case "SE_Thumb":
                            // check that you arent inverting the image (with a negative height)
                            if (move_to.Y - element.y_offset < 0)
                            {
                                break;
                            }

                            // check that you arent inverting the image (with a negative width)
                            if (move_to.X - element.x_offset < 0)
                            {
                                break;
                            }

                            ((Image_render_obj_display)child).Height = move_to.Y - element.y_offset;
                            ((Image_render_obj_display)child).Width = move_to.X - element.x_offset;
                            break;

                        case "NW_Thumb":

                            // check that you arent inverting the image (with a negative height)
                            if (((((Image_render_obj_display)child).sprite_viewbox.Height) + element.y_offset) - move_to.Y < 0)
                            {
                                break;
                            }

                            // check that you arent inverting the image (with a negative width)
                            if (((((Image_render_obj_display)child).sprite_viewbox.Width) + element.x_offset) - move_to.X < 0)
                            {
                                break;
                            }

                            Canvas.SetLeft(child, move_to.X);
                            Canvas.SetTop(child, move_to.Y);

                            ((Image_render_obj_display)child).Height = ((((Image_render_obj_display)child).sprite_viewbox.Height) + element.y_offset) - move_to.Y;
                            ((Image_render_obj_display)child).Width = ((((Image_render_obj_display)child).sprite_viewbox.Width) + element.x_offset) - move_to.X;
                            break;

                        case "NE_Thumb":

                            // check that you arent inverting the image (with a negative height)
                            if (((((Image_render_obj_display)child).sprite_viewbox.Height) + element.y_offset) - move_to.Y < 0)
                            {
                                break;
                            }

                            // check that you arent inverting the image (with a negative width)
                            if (move_to.X - element.x_offset < 0)
                            {
                                break;
                            }

                            Canvas.SetTop(child, move_to.Y);

                            ((Image_render_obj_display)child).Height = ((((Image_render_obj_display)child).sprite_viewbox.Height) + element.y_offset) - move_to.Y;
                            ((Image_render_obj_display)child).Width = move_to.X - element.x_offset;
                            break;
                    }
                }
            }
        }

        private void image_move(object[] data, Point move_to)
        {
            if (data[1] is not Image_render_obj_display || data[0] is not Point)
            {
                Debug.WriteLine("invalid selection entered image_scaling");
                return;
            }

            //state just before this one
            //Image_render_obj_display the_image_obj = (Image_render_obj_display)data[1];
            foreach (UIElement child in Object_display.Children)
            {
                if (child == data[1])
                {
                    Canvas.SetLeft(child, move_to.X - ((Point)data[0]).X);
                    Canvas.SetTop(child, move_to.Y - ((Point)data[0]).Y);

                }

            }
        }

        private void Object_display_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(object[])))
            {
                return;
            }
            // registers the dragover events on the main canvas
            object[] data = e.Data.GetData(typeof(object[])) as object[];
            Point move_to = e.GetPosition(Object_display);
            //Debug.WriteLine(sender);
            switch (data[data.Length - 1])
            {
                case "Image_scale":
                    Expanding_image(data, move_to);
                    break;
                case "Image_move":
                    image_move(data, move_to);
                    break;
            }
        }

        private void Finalise_obj_sprite_resize(object[] data)
        {
            if (data[1] is not Image_render_obj_display)
            {
                return;
            }


            //state just before this one
            Image_render_obj_display the_image_obj = (Image_render_obj_display)data[1];


            //Debug.WriteLine(e.Source);

            foreach (UIElement child in Object_display.Children)
            {
                if (child == data[1])
                {
                    




                    // change the actual data values that get saved
                    ((Sprite_renderer)Components_list.components[(int)the_image_obj.Tag]).x_offset = (int)Canvas.GetLeft((Image_render_obj_display)child);
                    ((Sprite_renderer)Components_list.components[(int)the_image_obj.Tag]).y_offset = (int)Canvas.GetTop((Image_render_obj_display)child);

                    ((Sprite_renderer)Components_list.components[(int)the_image_obj.Tag]).x_scale *= (((Image_render_obj_display)child).Width / ((Image_render_obj_display)child).sprite_viewbox.Width);
                    ((Sprite_renderer)Components_list.components[(int)the_image_obj.Tag]).y_scale *= (((Image_render_obj_display)child).Height / ((Image_render_obj_display)child).sprite_viewbox.Height);
                    Components_list.Reload_components();

                    // change the visual stretch values
                    ((Image_render_obj_display)child).sprite_viewbox.Width = ((Image_render_obj_display)child).Width;
                    ((Image_render_obj_display)child).sprite_viewbox.Height = ((Image_render_obj_display)child).Height;

                }
            }
        }

        private void Finalise_image_move(object[] data)
        {
            // is the final step to change the stored data of the images position ofter its been moved.
            if (data[1] is not Image_render_obj_display)
            {
                return;
            }
            //state just before this one
            Image_render_obj_display the_image_obj = (Image_render_obj_display)data[1];


            //Debug.WriteLine(e.Source);

            foreach (UIElement child in Object_display.Children)
            {
                if (child == data[1])
                {
                    

                    // change the actual data values that get saved
                    ((Sprite_renderer)Components_list.components[(int)the_image_obj.Tag]).x_offset = (int)Canvas.GetLeft((Image_render_obj_display)child);
                    ((Sprite_renderer)Components_list.components[(int)the_image_obj.Tag]).y_offset = (int)Canvas.GetTop((Image_render_obj_display)child);
                    Components_list.Reload_components();

                }
            }

        }

        private void Object_display_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(object[])))
            {
                object[] data = e.Data.GetData(typeof(object[])) as object[];
                switch (data[data.Length - 1]) {
                    case "Image_scale":
                        Finalise_obj_sprite_resize(data);
                        break;
                    case "Image_move":
                        Finalise_image_move(data);
                        break;
                }
            }
        }
    }
}
