using Game_Engine.faux_obj_types;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
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

namespace Game_Engine;

/// <summary>
/// Interaction logic for StartWindow.xaml
/// </summary>
public partial class StartWindow : Window
{


    public StartWindow()
    {
        InitializeComponent();
    }

    private void Open_proj(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Project_file|*.proj";
        openFileDialog.Title = "Open an existing project";
        openFileDialog.ShowDialog();

        if (openFileDialog.FileName != "")
        {
            Project_Window project = new Project_Window(openFileDialog.FileName);
            project.Show();
            this.Close();
        }
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

            string[] split_path = saveFileDialog1.FileName.Split("\\");

            Project empty_proj = new Project(split_path.Last(), "");
            string json_string = JsonConvert.SerializeObject(empty_proj);
            File.WriteAllText(saveFileDialog1.FileName, json_string);


            Directory.CreateDirectory(saveFileDialog1.FileName.Replace(split_path.Last(), "Objects"));
            Directory.CreateDirectory(saveFileDialog1.FileName.Replace(split_path.Last(), "Scripts"));
            Directory.CreateDirectory(saveFileDialog1.FileName.Replace(split_path.Last(), "Assets"));
            Directory.CreateDirectory(saveFileDialog1.FileName.Replace(split_path.Last(), "Places"));
            Directory.CreateDirectory(saveFileDialog1.FileName.Replace(split_path.Last(), "Shaders"));
            


            Project_Window project = new Project_Window(saveFileDialog1.FileName);
            project.Show();
            this.Close();
        }
    }
}