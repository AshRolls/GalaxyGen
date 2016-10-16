using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Compression;

namespace GalaxyGenCore.Framework
{
    public static class GeneralJsonSerializer
    {
        public static void SerializeAndSave(object o, string filePath)
        {
            JsonSerializer jsSer = new JsonSerializer();
            jsSer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            jsSer.Formatting = Formatting.None;
            jsSer.NullValueHandling = NullValueHandling.Ignore;

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                {
                    using (StreamWriter sw = new StreamWriter(gzip))
                    {
                        using (JsonWriter jsWriter = new JsonTextWriter(sw))
                        {
                            jsSer.Serialize(jsWriter, o);
                        }
                    }
                }
            }
        }

        public static T Deserialize<T>(string filePath, JsonConverter converter)
        {
            T obj = default(T);
            if (File.Exists(filePath))
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        using (StreamReader sr = new StreamReader(gzip))
                        {
                            using (JsonReader jr = new JsonTextReader(sr))
                            {
                                JsonSerializer jsSer = new JsonSerializer();
                                jsSer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                                jsSer.NullValueHandling = NullValueHandling.Ignore;
                                if (converter != null)
                                    jsSer.Converters.Add(converter);
                                obj = jsSer.Deserialize<T>(jr);
                            }
                        }
                    }
                }
            }
            return obj;
        }
    }

   


}
