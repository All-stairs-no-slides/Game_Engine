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

            path = path.Remove(path.LastIndexOf("\\") + 1);
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
                    node.Tag = dir;
                    if (files[i].Split("\\").Last().Split(".").Last() == "obj")
                    {
                        node.MouseDoubleClick += Obj_MouseDoubleClick;
                        
                    }
                    if (files[i].Split("\\").Last().Split(".").Last() == "place")
                    {
                        node.MouseDoubleClick += Place_MouseDoubleClick;
                    }
                    if (files[i].Split("\\").Last().Split(".").Last() == "spr")
                    {
                        node.MouseDoubleClick += Sprite_MouseDoubleClick;
                    }

                    // to drag and drop items in the solution
                    node.MouseMove += Solution_Item_Mouse_Move;

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
                    MenuItem Scene_Add = new MenuItem();
                    Scene_Add.Header = "Place";
                    Scene_Add.Click += new System.Windows.RoutedEventHandler(this.Create_Game_Place);
                    MenuItem Sprite_Add = new MenuItem();
                    Sprite_Add.Header = "Sprite";
                    Sprite_Add.Click += new System.Windows.RoutedEventHandler(this.Create_Game_Sprite);

                    // finalise  context menu
                    add_item.Items.Add(Object_add);
                    add_item.Items.Add(Scene_Add);
                    add_item.Items.Add(Sprite_Add);

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
                    node.Tag = dir;
                    parent.Items.Add(node);
                    add_dirs(dirs[i] + "\\", node);

                }
                add_files(dir, parent);
            }

            add_dirs(path_name);
        }

        private void Obj_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            TreeViewItem src_item = e.Source as TreeViewItem;
            string obj_name = src_item.Header.ToString();
            start_object_view(path + "\\Objects\\" + obj_name.ToString().Remove(obj_name.ToString().LastIndexOf(".")) + "\\" + obj_name);
        }

        private void Sprite_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            TreeViewItem src_item = e.Source as TreeViewItem;
            string spr_name = src_item.Header.ToString();
            start_sprite_view(path + "\\Assets\\" + spr_name.ToString().Remove(spr_name.ToString().LastIndexOf(".")) + "\\" + spr_name);
        }

        private void Place_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            TreeViewItem src_item = e.Source as TreeViewItem;
            string place_name = src_item.Header.ToString();
            Debug.Write(path + "Places\\" + place_name);

            start_place_view(path + "Places\\" + place_name);
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

        private void Create_Script(object sender, RoutedEventArgs e)
        {
            // Summary:
            // creates a python script for later use via an object or instance

            string Script_name = Interaction.InputBox("Script Name");

            if(Script_name == "")
            {
                return;
            }
            Directory.CreateDirectory(path + "\\Objects\\" + Script_name);



        }

        private void Create_Game_Sprite(object sender, RoutedEventArgs e)
        {
            // Summary:
            // creates a game object in the file system under the folder selected

            string Sprite_name = Interaction.InputBox("Sprite Name");
            //Debug.WriteLine(Obj_name);
            if (Sprite_name == "")
            {
                return;
            }
            Directory.CreateDirectory(path + "\\Assets\\" + Sprite_name);
            Game_Sprite Sprite = new Game_Sprite(new List<float> { }, new List<string> { }, Sprite_name);
            //Debug.WriteLine(obj.components[0].type);
            string json_string = JsonConvert.SerializeObject(Sprite);
            File.WriteAllText(path + "\\Assets\\" + Sprite_name + "\\" + Sprite_name + ".spr", json_string);
            Reload_Project_sol(sender, e);
        }

        private void Create_Game_Place(object sender, RoutedEventArgs e)
        {
            // Summary:
            // creates a game object in the file system under the folder selected

            string Place_Name = Interaction.InputBox("Place Name");
            //Debug.WriteLine(Obj_name);
            if (Place_Name == "")
            {
                return;
            }

            Game_Place place = new Game_Place(Place_Name, []);
            string json_string = JsonConvert.SerializeObject(place);

            File.WriteAllText(path + "\\Places\\" +  Place_Name + ".place", json_string);
            Reload_Project_sol(sender, e);
        }

        private void start_object_view(string Object_path)
        {
            // summary:
            // creates the object view window for the selected object
            ObjectViewWindow obj_window = new ObjectViewWindow(Object_path);
            obj_window.Show();
            
        }

        private void start_place_view(string Place_path)
        {
            PlaceViewWindow Place_window = new PlaceViewWindow(Place_path);
            Place_window.Show();
        }

        private void start_sprite_view(string sprite_path)
        {
            SpriteViewWindow Sprite_Window = new SpriteViewWindow(sprite_path);
            Sprite_Window.Show();
        }


        private void Solution_Item_Mouse_Move(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                //Debug.WriteLine(sender.GetType());
                TreeViewItem Item = sender as TreeViewItem;
                string path = Item.Tag.ToString() + Item.Header.ToString();
                DragDrop.DoDragDrop((TreeViewItem)sender, new DataObject(path), DragDropEffects.Copy);
            }
        }

        
    }
}
