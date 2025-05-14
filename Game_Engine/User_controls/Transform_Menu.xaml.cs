using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Transform_Menu.xaml
    /// </summary>
    public partial class Transform_Menu : UserControl
    {
        public Transform_Menu(int index, int x_pos, int y_pos ,int x_scale ,int y_scale )
        {


            this.Index = index;
            this.X_pos_prop = x_pos;
            this.Y_pos_prop = y_pos;
            this.X_scale_prop = x_scale;
            this.Y_scale_prop = y_scale;
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // index is used to find this component within the objects components array
        private int Index;

        private int X_pos;

        public int X_pos_prop
        {
            get { return X_pos; }
            set { X_pos = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X_pos_prop"));
                //find window that this is in to update the object component list for
                foreach (ObjectViewWindow window in Application.Current.Windows.OfType<ObjectViewWindow>())
                {
                    Debug.WriteLine(((ObjectViewWindow)window).Components_list.Tree_Parent.Items);
                    foreach (TreeViewItem item in ((ObjectViewWindow)window).Components_list.Tree_Parent.Items)
                    {
                        foreach(object content in item.Items)
                        {
                            if(content == this)
                            {
                                game_component[] comps = window.the_object.components;
                                ((transform_component)comps[Index]).x = value;
                            }
                        }
                    }

                        
                }
                //Debug.WriteLine(X_pos);
            }
        }

        private int Y_pos;

        public int Y_pos_prop
        {
            get { return Y_pos; }
            set { Y_pos = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y_pos_prop"));
                foreach (ObjectViewWindow window in Application.Current.Windows.OfType<ObjectViewWindow>())
                {
                    Debug.WriteLine(((ObjectViewWindow)window).Components_list.Tree_Parent.Items);
                    foreach (TreeViewItem item in ((ObjectViewWindow)window).Components_list.Tree_Parent.Items)
                    {
                        foreach (object content in item.Items)
                        {
                            if (content == this)
                            {
                                game_component[] comps = window.the_object.components;
                                ((transform_component)comps[Index]).y = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(Y_pos);
            }
        }

        private int X_scale;

        public int X_scale_prop
        {
            get { return X_scale; }
            set { X_scale = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X_scale_prop"));
                foreach (ObjectViewWindow window in Application.Current.Windows.OfType<ObjectViewWindow>())
                {
                    Debug.WriteLine(((ObjectViewWindow)window).Components_list.Tree_Parent.Items);
                    foreach (TreeViewItem item in ((ObjectViewWindow)window).Components_list.Tree_Parent.Items)
                    {
                        foreach (object content in item.Items)
                        {
                            if (content == this)
                            {
                                game_component[] comps = window.the_object.components;
                                ((transform_component)comps[Index]).x_scale = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_scale);
            }
        }

        private int Y_scale;

        public int Y_scale_prop
        {
            get { return Y_scale; }
            set { Y_scale = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y_scale_prop"));
                foreach (ObjectViewWindow window in Application.Current.Windows.OfType<ObjectViewWindow>())
                {
                    Debug.WriteLine(((ObjectViewWindow)window).Components_list.Tree_Parent.Items);
                    foreach (TreeViewItem item in ((ObjectViewWindow)window).Components_list.Tree_Parent.Items)
                    {
                        foreach (object content in item.Items)
                        {
                            if (content == this)
                            {
                                game_component[] comps = window.the_object.components;
                                ((transform_component)comps[Index]).y_scale = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(Y_scale);
            }
        }



    }
}
