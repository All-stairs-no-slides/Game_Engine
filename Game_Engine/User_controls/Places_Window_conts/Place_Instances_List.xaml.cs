using Game_Engine.faux_obj_types;
using Microsoft.VisualBasic;
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

        private void Add_Instance(object sender, RoutedEventArgs e)
        {
            string Obj_name = Interaction.InputBox("Instance Name");

            foreach (Window window in Application.Current.Windows.OfType<PlaceViewWindow>())
            {
                if (((PlaceViewWindow)window).Instance_list == this)
                {
                    ((PlaceViewWindow)window).Place.Instances = ((PlaceViewWindow)window).Place.Instances.Append(new Game_obj(Obj_name, [])).ToArray();
                    ((PlaceViewWindow)window).Add_Instance_to_Visual_List(new Game_obj(Obj_name, []), ((PlaceViewWindow)window).Place.Instances.Length - 1);
                    Debug.WriteLine("uouou");

                    Debug.WriteLine(((PlaceViewWindow)window).Place.Instances[((PlaceViewWindow)window).Place.Instances.Length - 1].Name);
                }
            }
           
        }

        private void Add_Asset(object sender, RoutedEventArgs e)
        {

        }
    }
}
