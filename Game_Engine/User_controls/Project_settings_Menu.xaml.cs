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
    /// Interaction logic for Project_settings_Menu.xaml
    /// </summary>
    public partial class Project_settings_Menu : UserControl
    {
        public Project_settings_Menu()
        {
            InitializeComponent();
            this.DataContext = this;

            //foreach (Project_Window window in Application.Current.Windows.OfType<Project_Window>())
            //{
            //    if (window.project_settings == this)
            //    {
            //        Window_Height_prop = window.project.Window_Height;
            //        Window_Width_prop = window.project.Window_Width;
            //        Viewport_Height_prop = window.project.Viewport_Height;
            //        Window_Width_prop = window.project.Window_Width;


            //    }
            //}
                     
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string Alias;
        public string Alias_prop
        {
            get { return Alias; }
            set
            {
                this.Alias = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Alias_prop"));

                foreach (Project_Window window in Application.Current.Windows.OfType<Project_Window>())
                {
                    if (window.project_settings == this)
                    {
                        window.project.Name = value;
                    }
                }
            }
        }

        private int Viewport_Width;
        public int Viewport_Width_prop
        { 
            get { return Viewport_Width; } 
            set 
            {
                this.Viewport_Width = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Viewport_Width_prop"));

                foreach(Project_Window window in Application.Current.Windows.OfType<Project_Window>())
                {
                    if(window.project_settings == this)
                    {
                        window.project.Viewport_Width = value;
                    }
                }
            }
        }

        private int Viewport_Height;
        public int Viewport_Height_prop
        {
            get { return Viewport_Height; }
            set
            {
                this.Viewport_Height = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Viewport_Height_prop"));

                foreach (Project_Window window in Application.Current.Windows.OfType<Project_Window>())
                {
                    if (window.project_settings == this)
                    {
                        window.project.Viewport_Height = value;
                    }
                }
            }
        }

        private int Window_Height;
        public int Window_Height_prop
        {
            get { return Window_Height; }
            set
            {
                this.Window_Height = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Window_Height_prop"));

                foreach (Project_Window window in Application.Current.Windows.OfType<Project_Window>())
                {
                    if (window.project_settings == this)
                    {
                        window.project.Window_Height = value;
                    }
                }
            }
        }

        private int Window_Width;
        public int Window_Width_prop
        {
            get { return Window_Width; }
            set
            {
                this.Window_Width = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Window_Width_prop"));

                foreach (Project_Window window in Application.Current.Windows.OfType<Project_Window>())
                {
                    if (window.project_settings == this)
                    {
                        window.project.Window_Width = value;
                    }
                }
            }
        }

    }
}
