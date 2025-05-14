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
            foreach (game_component comp in components)
            {
                Add_component_element(comp, index);
                index += 1;
            }
        }

        private void Add_component_element(game_component comp, int index)
        {
            switch (comp.type)
            {
                case "Transform":
                    transform_component component = (transform_component)comp;
                    TreeViewItem Transform_menu = new TreeViewItem();
                    
                    Transform_menu.Header = "Transform";
                    // populate menu
                    Transform_Menu content = new Transform_Menu(index, component.x, component.y, component.x_scale, component.y_scale);
                    Transform_menu.Items.Add(content);
                    Tree_Parent.Items.Add(Transform_menu);
                    break;
            }
        }

        private void Add_transform(object sender, RoutedEventArgs e)
        {
            if (components != null) {
                //find window that this is in to update the object component list for
                foreach (Window window in Application.Current.Windows.OfType<ObjectViewWindow>()) 
                {
                    if (((ObjectViewWindow)window).Components_list == this)
                    {
                        transform_component new_component = new transform_component("Transform", 0, 0, 1, 1);
                        ((ObjectViewWindow)window).the_object.components = ((ObjectViewWindow)window).the_object.components.Append(new_component).ToArray();
                        Add_component_element(new_component, ((ObjectViewWindow)window).the_object.components.Length - 1);

                        break;
                    } 
                }
                        
            }
        }
    }
}
