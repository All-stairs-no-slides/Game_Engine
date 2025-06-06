using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for Image_render_obj_display.xaml
    /// </summary>
    public partial class Image_render_obj_display : UserControl
    {
        public Image_render_obj_display()
        {
            InitializeComponent();
        }

        private void Thumb_MouseMove(object sender, MouseEventArgs e)
        {
            

            if (e.LeftButton == MouseButtonState.Pressed) 
            {
                Thumb thumb = sender as Thumb;
                string name = thumb.Name;
                //Debug.WriteLine(name);
                DragDrop.DoDragDrop(thumb, this, DragDropEffects.Move);
            }
        }
    }
}
