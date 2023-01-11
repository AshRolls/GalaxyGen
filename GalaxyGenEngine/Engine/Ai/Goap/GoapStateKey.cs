using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    public enum GoapStateKeyTypeEnum
    {
        StateName = 0,
        Resource = 1
    }

    public enum GoapStateKeyStateNameEnum
    {
        None,
        IsDocked,
        DockedAt
    }

    public struct GoapStateKey
    {
        public GoapStateKeyTypeEnum Type;
        public GoapStateKeyStateNameEnum StateName;
        public GoapStateKeyResLoc ResourceLocation;

        public GoapStateKey(GoapStateKeyTypeEnum type, GoapStateKeyStateNameEnum stateName, GoapStateKeyResLoc resourceLocation)
        {
            Type = type;
            StateName = stateName;
            ResourceLocation = resourceLocation;
        }
    }    

    public struct GoapStateKeyResLoc
    {
        public ResourceTypeEnum ResType;
        public long StoreId;

        public GoapStateKeyResLoc(ResourceTypeEnum resType, long storeId)
        {
            ResType = resType;
            StoreId = storeId;
        }
    }
}
