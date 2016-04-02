using GalaxyGen.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Framework
{
    public static class GalaxyJsonSerializer
    {
        public const string saveFile = @"C:\Space\galDbJson.gal";

        public static void SerializeAndSave(Galaxy gal)
        {            
            JsonSerializerSettings settings = new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
            string ser = JsonConvert.SerializeObject(gal, Formatting.Indented, settings); // TODO change formatting when we think we need a touch of extra speed
            File.WriteAllText(saveFile,ser);
        }

        public static Galaxy Deserialize()
        {
            Galaxy gal = null;
            if (File.Exists(saveFile))
            {
                String json = File.ReadAllText(saveFile);
                gal = JsonConvert.DeserializeObject<Galaxy>(json);                
            }
            return gal;
        }
    }
}
