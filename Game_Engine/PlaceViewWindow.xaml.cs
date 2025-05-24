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
using System.Windows.Shapes;

namespace Game_Engine
{
    /// <summary>
    /// Interaction logic for PlaceViewWindow.xaml
    /// </summary>
    public partial class PlaceViewWindow : Window
    {
        public PlaceViewWindow()
        {
            InitializeComponent();
        }

        private void Place_Window_Drop(object sender, DragEventArgs e)
        {
            Debug.WriteLine((string)e.Data.GetData(DataFormats.StringFormat));
        }
    }
}
