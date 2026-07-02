using Game_Engine.faux_obj_types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Collision_Menu.xaml
    /// </summary>
    public partial class Collision_Menu : UserControl
    {
        public Collision_Menu(int index, int[] proportions, string alias, string col_type)
        { 

            if(col_type == "Rect")
            {
                this.X_offset_prop = proportions[0];
                this.Y_offset_prop = proportions[1];
                this.Width_prop = proportions[2];
                this.Height_prop = proportions[3];
            };
            this.Index = index;
            this.Alias_prop = alias;
            this.Type_prop = col_type;
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // index is used to find this component within the objects components array
        private int Index;

        private int X_offset;

        public int X_offset_prop
        {
            get { return X_offset; }
            set
            {
                X_offset = value;

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
                                ((collider_component)comps[Index]).Proportions[0] = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

        private int Y_offset;

        public int Y_offset_prop
        {
            get { return Y_offset; }
            set
            {
                Y_offset = value;

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
                                ((collider_component)comps[Index]).Proportions[1] = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

        private int Width;

        public int Width_prop
        {
            get { return Width; }
            set
            {
                Width = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Width_prop"));
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
                                ((collider_component)comps[Index]).Proportions[2] = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

        private int Height;

        public int Height_prop
        {
            get { return Height; }
            set
            {
                Height = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Height_prop"));
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
                                ((collider_component)comps[Index]).Proportions[3] = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

        private string Alias;

        public string Alias_prop
        {
            get { return Alias; }
            set
            {
                Alias = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Alias_prop"));
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
                                ((collider_component)comps[Index]).Collider_alias = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

        private string Col_type;

        public string Type_prop
        {
            get { return Col_type; }
            set
            {
                Col_type = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Type_prop"));
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
                                ((collider_component)comps[Index]).Collider_type = value;
                            }
                        }
                    }


                }
                //Debug.WriteLine(X_pos);
            }
        }

    }
}
