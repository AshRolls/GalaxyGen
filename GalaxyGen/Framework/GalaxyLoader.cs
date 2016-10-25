using GalaxyGen.Model;
using GalaxyGenCore.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace GalaxyGen.Framework
{
    public static class GalaxyLoader
    {        
        public static void Save(Galaxy gal)
        {
            GeneralJsonSerializer.SerializeAndSave(gal, Globals.SERVER_SAVE_FILE);
        }

        public static Galaxy Load()
        {
            return GeneralJsonSerializer.Deserialize<Galaxy>(Globals.SERVER_SAVE_FILE, new GalaxyConverter());
        }
    }

    public class GalaxyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreLocation) || objectType == typeof(IAgentLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            object target;
            if (jo["$ref"] != null)
            {
                string id = (jo["$ref"] as JValue).Value as string;
                target = (object)serializer.ReferenceResolver.ResolveReference(serializer, id);
                if (target.GetType() == typeof(Planet)) return (Planet)target;
                else if (target.GetType() == typeof(Ship)) return (Ship)target;
                else if (target.GetType() == typeof(Agent)) return (Agent)target;
            }
            else
            {
                if (jo["GalType"].Value<Int64>() == (Int64)TypeEnum.Planet)
                    return jo.ToObject<Planet>(serializer);

                else if (jo["GalType"].Value<Int64>() == (Int64)TypeEnum.Ship)
                    return jo.ToObject<Ship>(serializer);

                else if (jo["GalType"].Value<Int64>() == (Int64)TypeEnum.Agent)
                    return jo.ToObject<Agent>(serializer);
            }

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


}
