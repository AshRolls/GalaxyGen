using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Framework
{
    public static class IdUtils
    {
        public static UInt64 currentId;

        public static UInt64 GetId()
        {
            return System.Threading.Interlocked.Increment(ref currentId);
        }

    }
}
