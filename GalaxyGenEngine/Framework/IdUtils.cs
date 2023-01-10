using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Framework
{
    public static class IdUtils
    {
        public static Int64 currentId;

        public static Int64 GetId()
        {
            return System.Threading.Interlocked.Increment(ref currentId);
        }

    }
}
