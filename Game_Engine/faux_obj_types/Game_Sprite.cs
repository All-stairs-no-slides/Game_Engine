using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.faux_obj_types
{
    class Game_Sprite
    {
        public string name {  get; set; }
        // the duration the frames
        public List<float> durations { get; set; }
        // the image locations in order
        public List<String> Images_location { get; set; }

        public Game_Sprite(List<float> durations, List<string> images_location, string name)
        {
            this.durations = durations;
            Images_location = images_location;
            this.name = name;
        }
    }
}
