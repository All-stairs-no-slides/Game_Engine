using Game_Engine.faux_obj_types;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Represents a single Game_obj instance on the PlaceViewWindow canvas.
    /// Displays only the sprite renderers, positioned relative to the instance transform.
    /// </summary>
    public partial class Instance_obj_display : UserControl
    {
        // Index of this instance in Place.Instances
        public int Instance_index;

        public Instance_obj_display()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads sprite images for this instance and adds them to Instance_canvas.
        /// Returns the (min_x, min_y) offset so PlaceViewWindow can adjust the
        /// container's canvas position to compensate for any negative sprite offsets.
        /// </summary>
        public (double min_x, double min_y) Load_sprites(Game_obj instance, string asset_path, double x_dpi_scale, double y_dpi_scale, double zoom)
        {
            Instance_canvas.Children.Clear();

            // Collect each sprite's rect so we can compute the bounding box
            var rects = new System.Collections.Generic.List<(Image img, double left, double top)>();

            double min_x = 0, min_y = 0, max_x = 0, max_y = 0;

            foreach (game_component component in instance.components)
            {
                if (component.GetType() != typeof(Sprite_renderer))
                {
                    continue;
                }

                Sprite_renderer spr = (Sprite_renderer)component;

                if (spr.Sprite_dir == "")
                {
                    continue;
                }

                string spr_path = asset_path + spr.Sprite_dir;
                if (!File.Exists(spr_path))
                {
                    Debug.WriteLine("Instance_obj_display: sprite file not found at " + spr_path);
                    continue;
                }

                string json_string = File.ReadAllText(spr_path);
                Game_Sprite the_sprite;
                try
                {
                    the_sprite = JsonConvert.DeserializeObject<Game_Sprite>(json_string);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Instance_obj_display: failed to deserialise sprite - " + ex.Message);
                    continue;
                }

                if (the_sprite == null || the_sprite.Images_location == null || the_sprite.Images_location.Count == 0)
                {
                    continue;
                }

                string img_path = asset_path + the_sprite.Images_location[0];
                if (!File.Exists(img_path))
                {
                    Debug.WriteLine("Instance_obj_display: image file not found at " + img_path);
                    continue;
                }

                BitmapImage bitmap = new BitmapImage();
                try
                {
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(img_path);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Instance_obj_display: failed to load image - " + ex.Message);
                    continue;
                }

                Image img = new Image();
                img.Source = bitmap;
                img.Width  = ((bitmap.PixelWidth  / x_dpi_scale) * zoom) * spr.x_scale;
                img.Height = ((bitmap.PixelHeight / y_dpi_scale) * zoom) * spr.y_scale;
                img.Stretch = Stretch.Fill;
                img.IsHitTestVisible = false;
                Canvas.SetZIndex(img, spr.depth);

                double left = (spr.x_offset / x_dpi_scale) * zoom;
                double top  = (spr.y_offset / y_dpi_scale) * zoom;

                // Expand bounding box
                if (left < min_x) min_x = left;
                if (top  < min_y) min_y = top;
                if (left + img.Width  > max_x) max_x = left + img.Width;
                if (top  + img.Height > max_y) max_y = top  + img.Height;

                rects.Add((img, left, top));
            }

            // If no sprites were loaded give the control a small minimum hit area
            // so it remains clickable and draggable on the canvas.
            double ctrl_w = max_x - min_x;
            double ctrl_h = max_y - min_y;
            if (ctrl_w < 10) ctrl_w = 10;
            if (ctrl_h < 10) ctrl_h = 10;

            this.Width  = ctrl_w;
            this.Height = ctrl_h;

            // Place each image relative to the UserControl's top-left (shifted by min)
            foreach (var (img, left, top) in rects)
            {
                Canvas.SetLeft(img, left - min_x);
                Canvas.SetTop(img,  top  - min_y);
                Instance_canvas.Children.Add(img);
            }

            return (min_x, min_y);
        }

        /// <summary>
        /// Highlights this instance as selected.
        /// </summary>
        public void Set_selected(bool selected)
        {
            Selection_border.BorderBrush = selected ? Brushes.CornflowerBlue : Brushes.Transparent;
        }

        private void Instance_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point drag_start = e.GetPosition(this);
                DragDrop.DoDragDrop(this, new object[] { drag_start, this, "Instance_move" }, DragDropEffects.Move);
            }
        }

        private void Instance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Selection is handled in PlaceViewWindow via the PreviewMouseLeftButtonDown
            // bubble — nothing extra needed here; just mark handled so the canvas does
            // not deselect immediately.
            e.Handled = true;

            // Notify the parent PlaceViewWindow to update selection
            foreach (Window window in Application.Current.Windows.OfType<PlaceViewWindow>())
            {
                if (Is_in_window(window))
                {
                    ((PlaceViewWindow)window).Select_instance(Instance_index);
                    break;
                }
            }
        }

        private bool Is_in_window(Window window)
        {
            // Walk the visual tree from this control upward to see if it belongs to 'window'
            DependencyObject current = this;
            while (current != null)
            {
                if (current == window) return true;
                current = VisualTreeHelper.GetParent(current);
            }
            return false;
        }
    }
}
