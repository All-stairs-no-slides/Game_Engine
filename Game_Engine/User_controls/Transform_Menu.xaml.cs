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

namespace Game_Engine.User_controls
{
    /// <summary>
    /// Interaction logic for Transform_Menu.xaml
    /// </summary>
    public partial class Transform_Menu : UserControl
    {
        public Transform_Menu()
        {
            DataContext = this;
            InitializeComponent();
        }

        private int X_pos;

        public int X_pos_prop
        {
            get { return X_pos; }
            set { X_pos = value;
                Debug.WriteLine(X_pos);
            }
        }

        private int Y_pos;

        public int Y_pos_prop
        {
            get { return Y_pos; }
            set { Y_pos = value; 
                Debug.WriteLine(Y_pos);
            }
        }

        private int X_scale;

        public int X_scale_prop
        {
            get { return X_scale; }
            set { X_scale = value; 
                Debug.WriteLine(X_scale);
            }
        }

        private int Y_scale;

        public int Y_scale_prop
        {
            get { return Y_scale; }
            set { Y_scale = value; 
                Debug.WriteLine(Y_scale);
            }
        }



    }
}
