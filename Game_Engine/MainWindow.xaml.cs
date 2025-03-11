using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Game_Engine;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Open_proj(object sender, RoutedEventArgs e)
    {

    }

    private void New_proj(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "Project_file|*.proj";
        saveFileDialog1.Title = "Create a new project";
        saveFileDialog1.ShowDialog();

        if(saveFileDialog1.FileName != "")
        {
            System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

            fs.Close();
        }
    }
}