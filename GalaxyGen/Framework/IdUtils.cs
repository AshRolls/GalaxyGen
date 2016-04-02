using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Framework
{
    public static class IdUtils
    {
        public static Int64 currentId { get; set; }

        public static Int64 GetId()
        {
            return currentId++;
        }

    }
}
