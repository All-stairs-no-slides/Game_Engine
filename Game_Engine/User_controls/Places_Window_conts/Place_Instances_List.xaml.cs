using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Game_Engine.faux_obj_types;

namespace Game_Engine.User_controls.Places_Window_conts
{
    /// <summary>
    /// Interaction logic for Place_Instances_List.xaml
    /// </summary>
    public partial class Place_Instances_List : UserControl
    {
        public Game_obj[] Place_objs;
        public Place_Instances_List()
        {
            InitializeComponent();
        }

        private void Add_obj_control(Game_obj Instance, int index)
        {
            // create the base menu that will be populated
            TreeViewItem The_menu = new TreeViewItem();

            The_menu.Header = Instance.Name;
            The_menu.Tag = index;

            The_menu.Items.Add(Instance);
            Tree_Parent.Items.Add(The_menu);
        }

        private void Add_Instance(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("adding");
        }

        private void Add_Asset(object sender, RoutedEventArgs e)
        {

        }
    }
}
