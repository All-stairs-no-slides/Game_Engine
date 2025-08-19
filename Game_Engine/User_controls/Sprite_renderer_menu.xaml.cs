using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Game_Engine.faux_obj_types;
using Microsoft.Win32;

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Sprite_renderer_menu.xaml
    /// </summary>
    public partial class Sprite_renderer_menu : UserControl
    {
        public Sprite_renderer_menu(int Index, int x_offset, int y_offset, double x_scale, double y_scale, double rotation, string sprite_path, int depth)
        {
            this.init = true;
            this.Index = Index;
            this.X_offset_prop = x_offset;
            this.Y_offset_prop = y_offset;
            this.X_scale_prop = x_scale;
            this.Y_scale_prop = y_scale;
            this.Sprite_path_prop = sprite_path;
            this.Rot_prop = rotation;
            this.Depth_prop = depth;
            this.init = false;
            DataContext = this;
            InitializeComponent();
        }
        private bool init;

        private void Sprite_Selection(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Sprite File|*.spr";
            openFileDialog.Title = "Choose a sprite";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)openFileDialog.OpenFile();
                this.Sprite_path_prop = fs.Name;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private int Index;

        private int depth;

        public int Depth_prop
        {
            get { return depth; }
            set
            {
                depth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Depth_prop"));
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
                                ((Sprite_renderer)comps[Index]).depth = value;
                            }
                        }
                    }

                    // set the visuals for the sprite renderer if the textbox feild is set
                    foreach (UIElement item in ((ObjectViewWindow)window).Object_display.Children)
                    {
                        // look through only Image renderers
                        if(item.GetType() != typeof(Image_render_obj_display))
                        {
                            continue;
                        }
                        // identify the correct sprite renderer
                        if((int)((Image_render_obj_display)item).Tag == Index)
                        {
                            Canvas.SetZIndex(item, value);  
                        }
                    }

                }
            }
        }

        private double rotation;
        
        public double Rot_prop
        {
            get { return rotation; }
            set { rotation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rot_prop"));
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
                                ((Sprite_renderer)comps[Index]).rotation = value;
                            }
                        }
                    }


                }
            }
        }


        private int x_offset;

        public int X_offset_prop
        {
            get { return x_offset; }
            set { x_offset = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X_offset_prop"));
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
                                ((Sprite_renderer)comps[Index]).x_offset = value;
                            }
                        }
                    }

                    // set the visuals for the sprite renderer if the textbox feild is set
                    foreach (UIElement item in ((ObjectViewWindow)window).Object_display.Children)
                    {
                        // look through only Image renderers
                        if (item.GetType() != typeof(Image_render_obj_display))
                        {
                            continue;
                        }
                        // identify the correct sprite renderer
                        if ((int)((Image_render_obj_display)item).Tag == Index)
                        {
                            if (init == true)
                            {
                                // for the initial set
                                continue;
                            }
                            Canvas.SetLeft(item, value);
                        }
                    }


                }
            }
        }

        private int y_offset;

        public int Y_offset_prop
        {
            get { return y_offset; }
            set { y_offset = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y_offset_prop"));
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
                                ((Sprite_renderer)comps[Index]).y_offset = value;
                            }
                        }
                    }

                    // set the visuals for the sprite renderer if the textbox feild is set
                    foreach (UIElement item in ((ObjectViewWindow)window).Object_display.Children)
                    {
                        // look through only Image renderers
                        if (item.GetType() != typeof(Image_render_obj_display))
                        {
                            continue;
                        }
                        // identify the correct sprite renderer
                        if ((int)((Image_render_obj_display)item).Tag == Index)
                        {
                            if (init == true)
                            {
                                // for the initial set
                                continue;
                            }
                            Canvas.SetTop(item, value);
                        }
                    }

                }
            }
        }

        private double x_scale;

        public double X_scale_prop
        {
            get { return x_scale; }
            set { 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X_scale_prop"));
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
                                ((Sprite_renderer)comps[Index]).x_scale = value;
                            }
                        }
                    }

                    // set the visuals for the sprite renderer if the textbox feild is set
                    foreach (UIElement item in ((ObjectViewWindow)window).Object_display.Children)
                    {
                        // look through only Image renderers
                        if (item.GetType() != typeof(Image_render_obj_display))
                        {
                            continue;
                        }
                        // identify the correct sprite renderer
                        if ((int)((Image_render_obj_display)item).Tag == Index)
                        {
                            if (init == true)
                            {
                                // for the initial set
                                continue;
                            }
                            ((Image_render_obj_display)item).Width = (((Image_render_obj_display)item).Width / x_scale) * value;
                            ((Image_render_obj_display)item).sprite_viewbox.Width = (((Image_render_obj_display)item).sprite_viewbox.Width / x_scale) * value;
                        }
                    }


                }
                x_scale = value;
            }
        }

        private double y_scale;

        public double Y_scale_prop
        {
            get { return y_scale; }
            set { 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y_scale_prop"));
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
                                ((Sprite_renderer)comps[Index]).y_scale = value;
                            }
                        }
                    }

                    // set the visuals for the sprite renderer if the textbox feild is set
                    foreach (UIElement item in ((ObjectViewWindow)window).Object_display.Children)
                    {
                        // look through only Image renderers
                        if (item.GetType() != typeof(Image_render_obj_display))
                        {
                            continue;
                        }
                        // identify the correct sprite renderer
                        if ((int)((Image_render_obj_display)item).Tag == Index)
                        {
                            if(init == true)
                            {
                                // for the initial set
                                continue;
                            }
                            ((Image_render_obj_display)item).Height = (((Image_render_obj_display)item).Height / y_scale) * value;
                            ((Image_render_obj_display)item).sprite_viewbox.Height = (((Image_render_obj_display)item).sprite_viewbox.Height / y_scale) * value;
                        }
                    }

                }
                y_scale = value;
            }
        }

        private string sprite_path;

        public string Sprite_path_prop
        {
            get { return sprite_path; }
            set { sprite_path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sprite_path_prop"));
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
                                ((Sprite_renderer)comps[Index]).Sprite_dir = value;
                            }
                        }
                    }
                }
            }
        }
    }
}
