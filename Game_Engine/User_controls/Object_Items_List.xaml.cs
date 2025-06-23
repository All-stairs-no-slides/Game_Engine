using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using Game_Engine.User_controls;
using Game_Engine.faux_obj_types;
using System.ComponentModel;

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Object_Items_List.xaml
    /// </summary>
    public partial class Object_Items_List : UserControl
    {
        public game_component[] components;

        public Object_Items_List()
        {
            InitializeComponent();
        }


        public void Reload_components()
        {
            foreach (Window window in Application.Current.Windows.OfType<ObjectViewWindow>())
                if (((ObjectViewWindow)window).Components_list == this)
                {
                    components = ((ObjectViewWindow)window).the_object.components;
                    break;
                }
            int index = 0;
            Tree_Parent.Items.Clear();
            foreach (game_component comp in components)
            {
                Add_component_element(comp, index);
                index += 1;
            }
        }

        private void Add_component_element(game_component comp, int index)
        {
            // create the base menu that will be populated
            TreeViewItem The_menu = new TreeViewItem();

            The_menu.Header = comp.type;
            The_menu.Tag = index;

            // populate the menu
            switch (comp.type)
            {
                case "Transform":
                    transform_component trans_component = (transform_component)comp;
                    
                    // populate menu
                    Transform_Menu trans_content = new Transform_Menu(index, trans_component.x, trans_component.y, trans_component.x_scale, trans_component.y_scale, trans_component.rotation, trans_component.z);
                    The_menu.Items.Add(trans_content);
                    Tree_Parent.Items.Add(The_menu);
                    break;

                case "Sprite_renderer":
                    Sprite_renderer spr_component = (Sprite_renderer)comp;

                    // populate menu
                    Sprite_renderer_menu spr_content = new Sprite_renderer_menu(index, spr_component.x_offset, spr_component.y_offset, spr_component.x_scale, spr_component.y_scale, spr_component.rotation, spr_component.Sprite_dir, spr_component.depth);
                    The_menu.Items.Add(spr_content);
                    Tree_Parent.Items.Add(The_menu);
                    break;
            }
        }

        private void add_blank_component(game_component blank)
        {
            // the template for each buttonpress for adding a new component to the object
            if (components != null)
            {
                //find window that this is in to update the object component list for
                foreach (Window window in Application.Current.Windows.OfType<ObjectViewWindow>())
                {
                    if (((ObjectViewWindow)window).Components_list == this)
                    {
                        ((ObjectViewWindow)window).the_object.components = ((ObjectViewWindow)window).the_object.components.Append(blank).ToArray();
                        Add_component_element(blank, ((ObjectViewWindow)window).the_object.components.Length - 1);
                        components = ((ObjectViewWindow)window).the_object.components;
                        break;
                    }
                }

            }
        }

        private void Add_transform(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < components.Length; i++) {
                if (components[i].GetType() == typeof(transform_component)) {
                    return;
                }
            }
            add_blank_component(new transform_component("Transform", 0, 0, 1, 1, 0, 0));
        }

        private void Add_Sprite_Renderer(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].GetType() == typeof(transform_component))
                {
                    add_blank_component(new Sprite_renderer("Sprite_renderer", 0, 0, 1, 1, 0, "", 1));
                    return;
                }
            }
            MessageBox.Show("you need a transform component to have a sprite renderer");
            
        }
    }
}
