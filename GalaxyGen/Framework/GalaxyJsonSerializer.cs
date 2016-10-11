using GalaxyGen.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace GalaxyGen.Framework
{
    public static class GalaxyJsonSerializer
    {
        public const string saveFile = @"C:\Space\galDbJson.gal";

        public static void SerializeAndSave(Galaxy gal)
        {         
            JsonSerializer jsSer = new JsonSerializer();
            jsSer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            using (StreamWriter sw = new StreamWriter(saveFile))
            {
                using (JsonWriter jsWriter = new JsonTextWriter(sw))
                {                    
                    jsSer.Serialize(jsWriter, gal);
                }                                              
            }            
        }


        public static Galaxy Deserialize()
        {
            Galaxy gal = null;
            if (File.Exists(saveFile))
            {
                using (StreamReader sr = new StreamReader(saveFile))
                {
                    using (JsonReader jr = new JsonTextReader(sr))
                    {
                        JsonSerializer jsSer = new JsonSerializer();
                        jsSer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                        jsSer.Converters.Add(new GalaxyConverter());
                        gal = jsSer.Deserialize<Galaxy>(jr);
                    }
                }
            }
            return gal;
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
            }
            else
            {
                if (jo["GalType"].Value<Int64>() == (Int64)TypeEnum.Planet)
                    return jo.ToObject<Planet>(serializer);

                if (jo["GalType"].Value<Int64>() == (Int64)TypeEnum.Ship)
                    return jo.ToObject<Ship>(serializer);
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
