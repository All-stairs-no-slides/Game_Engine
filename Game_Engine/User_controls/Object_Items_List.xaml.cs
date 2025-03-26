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

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Object_Items_List.xaml
    /// </summary>
    public partial class Object_Items_List : UserControl
    {
        public string Path;

        public Object_Items_List()
        {
            InitializeComponent();
        }


        private void Add_transform(object sender, RoutedEventArgs e)
        {
            if (Path != null) {
                Debug.WriteLine(Path);
                TreeViewItem Transform_menu = new TreeViewItem();
                Transform_menu.Header = "Transform";
                Transform_Menu content = new Transform_Menu();
                Transform_menu.Items.Add(content);
                Tree_Parent.Items.Add(Transform_menu);
            }
        }
    }
}
