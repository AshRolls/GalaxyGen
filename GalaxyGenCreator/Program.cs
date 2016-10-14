using GalaxyGenCore.Framework;
using GalaxyGenCore.StarChart;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenCreator
{
    class Program
    {
        // generates a json file that will be loaded in by galaxygencore to instantiate the star chart.
        static void Main(string[] args)
        {
            int numberOfSystems = 1;            
            if (args.Length == 1)            
            {
                int.TryParse(args[0], out numberOfSystems);                
            }

            Console.Out.WriteLine("Creating galaxy with {0} Solarsystems...", numberOfSystems.ToString());

            GalaxyCreator gc = new GalaxyCreator();            
            ScGalaxy gal = gc.GenerateGalaxy(numberOfSystems);

            GeneralJsonSerializer.SerializeAndSave(gal, Globals.GALAXY_CORE_DB);

            Console.Out.WriteLine("Json saved as {0}", Globals.GALAXY_CORE_DB);
        }

        public static void SerializeAndSave(ScGalaxy gal)
        {
            JsonSerializer jsSer = new JsonSerializer();
            jsSer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            jsSer.Formatting = Formatting.None;
            jsSer.NullValueHandling = NullValueHandling.Ignore;

            using (FileStream stream = new FileStream(Globals.GALAXY_CORE_DB, FileMode.Create, FileAccess.Write))
            {
                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                {
                    using (StreamWriter sw = new StreamWriter(gzip))
                    {
                        using (JsonWriter jsWriter = new JsonTextWriter(sw))
                        {
                            jsSer.Serialize(jsWriter, gal);
                        }
                    }
                }
            }
        }

    }
}
