using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Framework
{
    public static class RandomUtils
    {
        // Function to get random number
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public static int RandomRange(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        public static int Random(int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(max);
            }
        }

    }
}
