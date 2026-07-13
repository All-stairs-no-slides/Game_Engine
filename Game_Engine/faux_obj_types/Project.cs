using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.faux_obj_types
{
    public class Project
    {

        public string Name { get; set; }

        public string Start_place { get; set; }
        public int num_of_builds { get; set; }
        public int Window_Width { get; set; }
        public int Window_Height { get; set; }
        public int Viewport_Width { get; set; }
        public int Viewport_Height { get; set; }




        public Project(string name, string start_place, int window_width, int window_height, int viewport_width, int viewport_height, int build_count = 0) { 
            Name = name;
            Start_place = start_place;
            num_of_builds = build_count;
            Window_Width = window_width;
            Window_Height = window_height;
            Viewport_Height = viewport_height;
            Viewport_Width = viewport_width;
        }
    }
}
