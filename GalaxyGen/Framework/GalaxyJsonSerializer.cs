using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Framework
{
    public static class GalaxyJsonSerializer
    {
        public static String Serialize(List<int> vals)
        {
            return JsonConvert.SerializeObject(vals);
        }

        public static List<int> Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<List<int>>(json);
        }
    }
}
