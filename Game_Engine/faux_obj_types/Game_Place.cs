using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.faux_obj_types
{
    public class Game_Place
    {
        public string Place_name { get; set; }
        public Game_obj[] Instances { get; set; }
        public Game_Place(string name, Game_obj[] instances)
        {
            this.Instances = instances;
            this.Place_name = name;

            //if (instances.Length == 0)
            //{
            //    Game_obj background = new Game_obj("background", []);
            //    this.Instances = [background];
            //}
        }
    }
}
