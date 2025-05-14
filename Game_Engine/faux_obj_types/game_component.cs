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
                    transform_component temp = new transform_component("Transform" , 0, 0, 1, 1);
                    serializer.Populate(jsonObject.CreateReader(), temp);
                    Debug.WriteLine(temp.x);

                    return temp;

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
                    transform_component casted_val = (transform_component)value;
                    jo.Add("x", casted_val.x);
                    jo.Add("y", casted_val.y);
                    jo.Add("x_scale", casted_val.x_scale);
                    jo.Add("y_scale", casted_val.y_scale);

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
        public int x_scale { get; set; }
        public int y_scale { get; set; }

        public transform_component(string type, int X, int Y, int X_scale, int Y_scale)
        {
            this.type = type;
            this.x = X;
            this.y = Y;
            this.x_scale = X_scale;
            this.y_scale = Y_scale;
        }
    }
}
