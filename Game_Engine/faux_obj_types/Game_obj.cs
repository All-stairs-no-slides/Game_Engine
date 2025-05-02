using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.faux_obj_types;
namespace Game_Engine.faux_obj_types
{
    class Game_obj
    {
        public string Name { get; set; }
        public game_component[] components { get; set; }

        public Game_obj(string name, game_component[] components)
        {
            this.Name = name;
        }
    }
}
