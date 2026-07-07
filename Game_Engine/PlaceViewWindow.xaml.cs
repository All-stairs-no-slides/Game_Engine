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
using Game_Engine.User_controls;
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

        public double zoom = 0.5;
        private double x_dpi_scale = 1.0;
        private double y_dpi_scale = 1.0;
        private string asset_path = "";

        // Index of the currently selected instance (-1 = none)
        private int selected_instance_index = -1;

        public PlaceViewWindow(string place_path)
        {
            this.path = place_path;
            string jsonString = File.ReadAllText(place_path);
            Place = JsonConvert.DeserializeObject<Game_Place>(jsonString)!;
            InitializeComponent();

            // add all of the Instances to the list on startup of window
            int index = 0;
            foreach (Game_obj inst in Place.Instances)
            {
                Add_Instance_to_Visual_List(inst, index);
                index++;
            }
        }

        // ─── Loaded ────────────────────────────────────────────────────────────

        private void Place_Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Grab DPI scale from the canvas
            PresentationSource psource = PresentationSource.FromVisual(Place_display);
            if (psource != null)
            {
                Matrix dpi_trans = psource.CompositionTarget.TransformToDevice;
                x_dpi_scale = dpi_trans.M11;
                y_dpi_scale = dpi_trans.M22;
            }

            // Grab project asset path from the open Project_Window
            foreach (Project_Window win in Application.Current.Windows.OfType<Project_Window>())
            {
                asset_path = win.path + "Assets\\";
            }

            Load_Place_Visuals();
        }

        // ─── Visual display ────────────────────────────────────────────────────

        /// <summary>
        /// Populates Place_display with an Instance_obj_display for every instance.
        /// </summary>
        private void Load_Place_Visuals()
        {
            Place_display.Children.Clear();

            int index = 0;
            foreach (Game_obj inst in Place.Instances)
            {
                Add_Instance_to_Place_display(inst, index);
                index++;
            }
        }

        /// <summary>
        /// Creates and positions a single Instance_obj_display on Place_display.
        /// </summary>
        private void Add_Instance_to_Place_display(Game_obj inst, int index)
        {
            // components[0] is always the transform
            if (inst.components == null || inst.components.Length == 0 ||
                inst.components[0].GetType() != typeof(transform_component))
            {
                return;
            }

            transform_component t = (transform_component)inst.components[0];

            Instance_obj_display inst_display = new Instance_obj_display();
            inst_display.Instance_index = index;
            var (min_x, min_y) = inst_display.Load_sprites(inst, asset_path, x_dpi_scale, y_dpi_scale, zoom);

            // Position by transform x/y, compensating for any negative sprite offsets
            // that were normalised inside Load_sprites (the container is shifted by min_x/min_y
            // so the sprite that sits furthest top-left is at the UserControl origin).
            Canvas.SetLeft(inst_display, (t.x / x_dpi_scale) * zoom + min_x);
            Canvas.SetTop(inst_display,  (t.y / y_dpi_scale) * zoom + min_y);
            Canvas.SetZIndex(inst_display, t.z);

            // Apply scale and rotation from transform
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform(t.x_scale, t.y_scale));
            tg.Children.Add(new RotateTransform(t.rotation));
            inst_display.RenderTransform = tg;

            Place_display.Children.Add(inst_display);
        }

        // ─── Selection ─────────────────────────────────────────────────────────

        /// <summary>
        /// Called by Instance_obj_display when clicked; updates the selection highlight.
        /// </summary>
        public void Select_instance(int index)
        {
            // Clear old selection
            foreach (UIElement child in Place_display.Children)
            {
                if (child is Instance_obj_display d)
                {
                    d.Set_selected(false);
                }
            }

            selected_instance_index = index;

            // Highlight the newly selected one
            foreach (UIElement child in Place_display.Children)
            {
                if (child is Instance_obj_display d && d.Instance_index == index)
                {
                    d.Set_selected(true);
                    break;
                }
            }
        }

        // ─── Instance list sidebar ─────────────────────────────────────────────

        private Game_obj Get_Instance(string Obj_path)
        {
            string jsonString = File.ReadAllText(Obj_path);
            return JsonConvert.DeserializeObject<Game_obj>(jsonString)!;
        }

        public void Add_Instance_to_Visual_List(Game_obj inst, int index)
        {
            TreeViewItem Instance_List_Item = new TreeViewItem();
            Instance_List_Item.Header = inst.Name;
            Instance_List_Item.Tag = index;
            Instance_List_Item.MouseDoubleClick += Open_Instance_Window;
            Instance_list.Tree_Parent.Items.Add(Instance_List_Item);
        }

        // ─── Drag-and-drop: dropping .obj onto the window ──────────────────────

        private void Place_Window_Drop(object sender, DragEventArgs e)
        {
            // Only handle string drops (dragged from solution explorer)
            if (!e.Data.GetDataPresent(DataFormats.StringFormat)) return;

            string dropped = (string)e.Data.GetData(DataFormats.StringFormat);
            if (dropped.Split("\\").Last().Split(".").Last() != "obj") return;

            Game_obj Instance = Get_Instance(dropped);

            if (Place.Instances != null)
            {
                Place.Instances = Place.Instances.Append(Instance).ToArray();
            }
            else
            {
                Place.Instances = [Instance];
            }

            int new_index = Place.Instances.Length - 1;
            Add_Instance_to_Visual_List(Instance, new_index);
            Add_Instance_to_Place_display(Instance, new_index);
        }

        // ─── Drag-and-drop: moving instances on Place_display ──────────────────

        private void Place_display_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(object[]))) return;

            object[] data = e.Data.GetData(typeof(object[])) as object[];
            if (data == null || data.Length < 3) return;

            Point move_to = e.GetPosition(Place_display);

            switch (data[data.Length - 1])
            {
                case "Instance_move":
                    Instance_Move(data, move_to);
                    break;
            }
        }

        private void Place_display_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(object[])))
            {
                object[] data = e.Data.GetData(typeof(object[])) as object[];
                if (data == null || data.Length < 3) return;

                switch (data[data.Length - 1])
                {
                    case "Instance_move":
                        Finalise_Instance_Move(data);
                        break;
                }
            }

            // Also handle string drops (obj files dragged from solution explorer)
            // that bubble up from the window-level drop handler
        }

        /// <summary>
        /// Updates the visual position of the instance during drag.
        /// data[0] = Point (drag-start offset within the control)
        /// data[1] = Instance_obj_display being dragged
        /// </summary>
        private void Instance_Move(object[] data, Point move_to)
        {
            if (data[1] is not Instance_obj_display || data[0] is not Point) return;

            Instance_obj_display inst_display = (Instance_obj_display)data[1];
            Point drag_offset = (Point)data[0];

            double new_left = move_to.X - drag_offset.X;
            double new_top  = move_to.Y - drag_offset.Y;

            Canvas.SetLeft(inst_display, new_left);
            Canvas.SetTop(inst_display,  new_top);
        }

        /// <summary>
        /// Writes the final canvas position back into the instance's transform on drop.
        /// </summary>
        private void Finalise_Instance_Move(object[] data)
        {
            if (data[1] is not Instance_obj_display) return;

            Instance_obj_display inst_display = (Instance_obj_display)data[1];
            int idx = inst_display.Instance_index;

            if (idx < 0 || idx >= Place.Instances.Length) return;
            if (Place.Instances[idx].components == null ||
                Place.Instances[idx].components.Length == 0 ||
                Place.Instances[idx].components[0].GetType() != typeof(transform_component)) return;

            transform_component t = (transform_component)Place.Instances[idx].components[0];

            // Convert canvas coords back to raw values
            t.x = (int)(Canvas.GetLeft(inst_display) * x_dpi_scale / zoom);
            t.y = (int)(Canvas.GetTop(inst_display)  * y_dpi_scale / zoom);
        }

        // ─── Open instance ObjectViewWindow ────────────────────────────────────

        private void Open_Instance_Window(object sender, MouseButtonEventArgs e)
        {
            int window_index = 0;
            foreach (Window window in Application.Current.Windows.OfType<PlaceViewWindow>())
            {
                if (window == this)
                {
                    TreeViewItem source_item = e.Source as TreeViewItem;
                    ObjectViewWindow Instance_window = new ObjectViewWindow(
                        Place.Instances[(int)source_item.Tag],
                        (int)source_item.Tag,
                        window_index);
                    Instance_window.Show();
                    break;
                }
                window_index++;
            }
        }

        // ─── Save ──────────────────────────────────────────────────────────────

        public void Save_Place()
        {
            string json_string = JsonConvert.SerializeObject(Place);
            json_string = json_string.Replace("\"[", "[");
            json_string = json_string.Replace("]\"", "]");
            File.WriteAllText(path, json_string);
        }

        // ─── Key handling ──────────────────────────────────────────────────────

        private void Key_pressed(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Save_Place();
            }
            else if (e.Key == Key.Delete)
            {
                // Try to delete from the sidebar list first if something is selected there
                if (Instance_list.Tree_Parent.SelectedItem != null)
                {
                    TreeViewItem deletion_item = (TreeViewItem)Instance_list.Tree_Parent.SelectedItem;
                    int deletion_index = (int)deletion_item.Tag;
                    Remove_instance(deletion_index);
                    Instance_list.Tree_Parent.Items.Remove(deletion_item);
                }
                else if (selected_instance_index >= 0)
                {
                    // Delete the visually selected instance
                    Remove_instance(selected_instance_index);

                    // Remove the corresponding sidebar item
                    foreach (TreeViewItem item in Instance_list.Tree_Parent.Items)
                    {
                        if ((int)item.Tag == selected_instance_index)
                        {
                            Instance_list.Tree_Parent.Items.Remove(item);
                            break;
                        }
                    }

                    selected_instance_index = -1;
                }
            }
        }

        /// <summary>
        /// Removes an instance from Place.Instances and from Place_display.
        /// </summary>
        private void Remove_instance(int index)
        {
            Place.Instances = Place.Instances.Where((val, i) => i != index).ToArray();

            // Remove the visual from the canvas
            UIElement to_remove = null;
            foreach (UIElement child in Place_display.Children)
            {
                if (child is Instance_obj_display d && d.Instance_index == index)
                {
                    to_remove = child;
                    break;
                }
            }
            if (to_remove != null)
            {
                Place_display.Children.Remove(to_remove);
            }

            // Shift Instance_index values on remaining displays down by 1 if needed
            foreach (UIElement child in Place_display.Children)
            {
                if (child is Instance_obj_display d && d.Instance_index > index)
                {
                    d.Instance_index--;
                }
            }
        }

        // ─── Misc ──────────────────────────────────────────────────────────────

        private void set_as_start(object sender, RoutedEventArgs e)
        {
            foreach (Project_Window win in Application.Current.Windows.OfType<Project_Window>())
            {
                win.project.Start_place = this.Place.Place_name;
            }
        }

        private void Instance_list_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
