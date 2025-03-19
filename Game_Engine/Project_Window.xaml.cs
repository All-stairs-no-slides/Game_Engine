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
using Game_Engine.User_controls;
using Microsoft.VisualBasic;


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
                
                // it will always be the general case since dirs are dealt with first
                for (int i = 0; i < files.Length; i++)
                {
                    TreeViewItem node = new TreeViewItem();
                    node.Header = files[i].Split("\\").Last();
                    parent.Items.Add(node);
                }
            }

            void add_dirs(string dir, TreeViewItem parent = null)
            {

                string[] dirs = Directory.GetDirectories(dir);
                // initial case and starting case for any reloads
                if (parent == null)
                {

                    TreeViewItem node = new TreeViewItem();

                    node.Header = "Project Solution";

                    // build the right click menu
                    ContextMenu new_menu = new ContextMenu();
                    node.ContextMenu = new_menu;
                    MenuItem add_item = new MenuItem();
                    add_item.Header = "Add";
                    MenuItem Object_add = new MenuItem();
                    Object_add.Header = "Object";
                    Object_add.Click += new System.Windows.RoutedEventHandler(this.Create_Game_Object);

                    // finalise  context menu
                    add_item.Items.Add(Object_add);
                    new_menu.Items.Add(add_item);

                    // add dir to the tree 
                    sol_exp_tree.Items.Add(node);

                    // callback to self and then add the files in this dir
                    add_dirs(path_name, node);
                    add_files(path_name, node);
                    return;
                }

                // general case for when theres a parent
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

        private void Reload_Project_sol(object sender, RoutedEventArgs e)
        {
            sol_exp_tree.Items.Clear();
            Refresh_sol_exp(path);
        }

        private void Create_Game_Object(object sender, RoutedEventArgs e)
        {
            // Summary:
            // creates a game object in the file system under the folder selected

            string Obj_name = Interaction.InputBox("Object Name");
            Debug.WriteLine(Obj_name);
            if(Obj_name == "") {
                return;
            }
            Directory.CreateDirectory(path + "\\Objects\\" + Obj_name);
            Reload_Project_sol(sender, e);
        }
    }
}
