using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Game_Engine.faux_obj_types
{
 //!!!!!!!! note: that for each added component type, cases should be added to the ReadJson() and WriteJson functions
    public class GameComponentConverter : Newtonsoft.Json.JsonConverter<game_component>
    {
        public override game_component ReadJson(JsonReader reader, Type objectType, game_component existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
            var type = jsonObject["type"]?.ToString();

            switch (type)
            {
                case "Transform":
                    transform_component trans_temp = new transform_component("Transform" , 0, 0, 1, 1, 0, 0);
                    serializer.Populate(jsonObject.CreateReader(), trans_temp);
                    Debug.WriteLine(trans_temp.x);

                    return trans_temp;

                case "Sprite_renderer":
                    Sprite_renderer spr_temp = new Sprite_renderer("Sprite_renderer", 0, 0, 1, 1, 0, "");
                    serializer.Populate(jsonObject.CreateReader(), spr_temp);
                    Debug.WriteLine(spr_temp.x_offset);

                    return spr_temp;

                case null:
                case "":
                    throw new JsonSerializationException("Missing or empty 'type' field in component.");

                default:
                    Console.WriteLine($"Warning: Unknown component type '{type}'. Falling back to base type.");
                    return jsonObject.ToObject<game_component>(serializer);
            }


        }

        public override void WriteJson(JsonWriter writer, game_component value, JsonSerializer serializer)
        {
            JObject jo = new JObject();
            jo.Add("type", value.type);
            switch (value.type)
            {

                case "Transform":
                    transform_component trans_val = (transform_component)value;
                    jo.Add("x", trans_val.x);
                    jo.Add("y", trans_val.y);
                    jo.Add("z", trans_val.z);
                    jo.Add("x_scale", trans_val.x_scale);
                    jo.Add("y_scale", trans_val.y_scale);
                    jo.Add("rotation", trans_val.rotation);


                    break;

                case "Sprite_renderer":
                    Sprite_renderer sprite_val = (Sprite_renderer)value;
                    jo.Add("x_offset", sprite_val.x_offset);
                    jo.Add("y_offset", sprite_val.y_offset);
                    jo.Add("x_scale", sprite_val.x_scale);
                    jo.Add("y_scale", sprite_val.y_scale);
                    jo.Add("Sprite_dir", sprite_val.Sprite_dir);
                    jo.Add("rotation", sprite_val.rotation);


                    break;


            }
            jo.WriteTo(writer);
        }
    }

    [Newtonsoft.Json.JsonConverter(typeof(GameComponentConverter))]
    public class game_component
    {
        public string type { get; set; }
        
    }

    public class transform_component : game_component
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int x_scale { get; set; }
        public int y_scale { get; set; }
        public int rotation { get; set; }
        public transform_component(string type, int X, int Y, int X_scale, int Y_scale, int rotation, int Z)
        {
            this.type = type;
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.x_scale = X_scale;
            this.y_scale = Y_scale;
            this.rotation = rotation;
        }
    }

    public class Sprite_renderer : game_component
    {
        public int x_offset { get; set; }
        public int y_offset { get; set; }
        public double x_scale { get; set; }
        public double y_scale { get; set; }
        public double rotation { get; set; }
        public string Sprite_dir { get; set; }
        public Sprite_renderer(string type, int x_offset, int y_offset, double x_scale, double y_scale, double rotation, string sprite_dir)
        {
            this.type = type;
            this.x_offset = x_offset;
            this.y_offset = y_offset;
            this.x_scale = x_scale;
            this.y_scale = y_scale;
            this.rotation = rotation;
            this.Sprite_dir = sprite_dir;
        }
    }
}
