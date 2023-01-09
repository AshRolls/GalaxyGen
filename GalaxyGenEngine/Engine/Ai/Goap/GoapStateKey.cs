using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    public enum GoapStateKeyEnum
    {
        String = 0,
        Resource = 1
    }
    public struct GoapStateKey
    {
        public GoapStateKeyEnum Type;
        public string String;
        public ResourceTypeEnum ResType;
        public long StoreId;
    }    
}
