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
        ResourceQty = 1,
        AllowedResource = 2
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
        public ResourceTypeEnum AllowedResource;

        public GoapStateKey(GoapStateKeyTypeEnum type, GoapStateKeyStateNameEnum stateName, GoapStateKeyResLoc resourceLocation, ResourceTypeEnum allowedResource)
        {
            Type = type;
            StateName = stateName;
            ResourceLocation = resourceLocation;
            AllowedResource = allowedResource;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            switch (Type)
            {
                case GoapStateKeyTypeEnum.StateName:
                    sb.Append("State: ");
                    sb.Append(StateName.ToString());                    
                    break;
                case GoapStateKeyTypeEnum.ResourceQty:
                    sb.Append("Resource: ");
                    sb.Append(ResourceLocation.ResType.ToString());
                    sb.Append(' ');
                    sb.Append(ResourceLocation.StoreId);
                    break;
                case GoapStateKeyTypeEnum.AllowedResource:
                    sb.Append("Allowed Resource ");                    
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
