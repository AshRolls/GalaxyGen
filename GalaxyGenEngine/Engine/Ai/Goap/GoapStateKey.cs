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
        DockedAt,
        ShipStoreId
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            switch (Type)
            {
                case GoapStateKeyTypeEnum.StateName:
                    sb.Append("State: ");
                    sb.Append(StateName.ToString());                    
                    break;
                case GoapStateKeyTypeEnum.Resource:
                    sb.Append("Resource: ");
                    sb.Append(ResourceLocation.ResType.ToString());
                    sb.Append(" ");
                    sb.Append(ResourceLocation.StoreId);
                    break;
            }
            return sb.ToString();
        }
    }    

    public struct GoapStateKeyResLoc
    {
        public ResourceTypeEnum ResType;
        public ulong StoreId;

        public GoapStateKeyResLoc(ResourceTypeEnum resType, ulong storeId)
        {
            ResType = resType;
            StoreId = storeId;
        }
    }
}
