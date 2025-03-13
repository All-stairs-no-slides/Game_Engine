using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for Project_Window.xaml
    /// </summary>
    public partial class Project_Window : Window
    {
        System.IO.FileStream Project_file;
        string path;
        public Project_Window(System.IO.FileStream Project_file)
        {
            InitializeComponent();
            this.Project_file = Project_file;
            path = Project_file.Name;
            this.Project_file.Close();

            path = path.Remove(path.LastIndexOf("\\"));
            // init solution explorer
            Refresh_sol_exp(path);
        }

        private void Refresh_sol_exp(string path_name)
        {
 
            void add_files(string dir, TreeViewItem parent = null)
            {

                string[] files = Directory.GetFiles(dir);
                if (parent == null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        sol_exp_tree.Items.Add(files[i].Split("\\").Last());
                    }
                    return;
                }

                for (int i = 0; i < files.Length; i++)
                {
                    parent.Items.Add(files[i].Split("\\").Last());
                }
            }

            void add_dirs(string dir, TreeViewItem parent = null)
            {

                string[] dirs = Directory.GetDirectories(dir);
                if (parent == null)
                {
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        Debug.WriteLine(dirs[i]);

                        TreeViewItem node = new TreeViewItem();
                        node.Header = dirs[i].Split("\\").Last();
                        sol_exp_tree.Items.Add(node);
                        add_dirs(dirs[i] + "\\", node);

                    }
                    add_files(dir);
                    return;
                }

                //Debug.WriteLine(Directory.GetDirectories(path_name).Length);
                for (int i = 0; i < dirs.Length; i++)
                {
                    TreeViewItem node = new TreeViewItem();
                    node.Header = dirs[i].Split("\\").Last();
                    parent.Items.Add(node);
                    add_dirs(dirs[i] + "\\", node);

                }
                add_files(dir, parent);
            }

            add_dirs(path_name);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sol_exp_tree.Items.Clear();
            Refresh_sol_exp(path);
        }
    }
}
