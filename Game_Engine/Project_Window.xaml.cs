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
using Game_Engine.faux_obj_types;
using Newtonsoft.Json;


namespace Game_Engine
{
    /// <summary>
    /// Interaction logic for Project_Window.xaml
    /// </summary>
    public partial class Project_Window : Window
    {
        private System.IO.FileStream Project_file;
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
                    if (files[i].Split("\\").Last().Split(".").Last() == "obj")
                    {
                        node.MouseDoubleClick += Node_MouseDoubleClick;
                    }

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

                    // callback to self to add proper files and dirs
                    add_dirs(path_name, node);
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

        private void Node_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            TreeViewItem src_item = e.Source as TreeViewItem;
            string obj_name = src_item.Header.ToString();
            start_object_view(path + "\\Objects\\" + obj_name.ToString().Remove(obj_name.ToString().LastIndexOf(".")) + "\\" + obj_name);
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
            //Debug.WriteLine(Obj_name);
            if(Obj_name == "") {
                return;
            }
            Directory.CreateDirectory(path + "\\Objects\\" + Obj_name);
            Game_obj obj = new Game_obj(Obj_name, []);
            Debug.WriteLine(obj.components[0].type);
            string json_string = JsonConvert.SerializeObject(obj);
            File.WriteAllText(path + "\\Objects\\" + Obj_name + "\\" + Obj_name + ".obj", json_string);
            Reload_Project_sol(sender, e);
        }

        private void start_object_view(string Object_path)
        {
            // summary:
            // creates the object view window for the selected object
            ObjectViewWindow obj_window = new ObjectViewWindow(Object_path);
            obj_window.Show();
            
        }
    }
}
