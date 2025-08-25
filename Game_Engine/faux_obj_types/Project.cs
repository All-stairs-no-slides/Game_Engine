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

        public Project(string name, string start_place, int build_count = 0) { 
            Name = name;
            Start_place = start_place;
            num_of_builds = build_count;
        }
    }
}
