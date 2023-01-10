using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenCore.Resources
{
    public enum ResourceTypeEnum : ulong
    {
        NotSet = 0,
        Spice = 1,
        Platinum = 2
    }

    public struct ResourceQuantity
    {
        public ResourceQuantity(ResourceTypeEnum type, ulong qty)
        {
            Type = type;
            Quantity = qty;
        }

        public ResourceTypeEnum Type { get; set; }
        public ulong Quantity { get; set; }
    }

    public class ResourceType
    {
        public ResourceTypeEnum Type { get; set; }
        public String Name { get; set; }
        public Double VolumePerUnit { get; set; }
        public Int64 DefaultTicksToProduce { get; set; }
    }

    public static class ResourceTypes
    {
        private static ResourceType[] types_Var;
        public static ResourceType[] Types
        {
            get
            {
                if (types_Var == null) types_Var = new ResourceType[Enum.GetNames(typeof(ResourceTypeEnum)).Length]; // initialise to size of enum
                return types_Var;
            }
            set
            {
                types_Var = value;
            }
        }

        public static ResourceType GetResource(ResourceTypeEnum resType)
        {
            int resIdx = (int)resType;
            return types_Var[resIdx];
        }

        public static void InitialiseResourceTypes()
        {
            // TODO Json ify ?
            ResourceTypes.Types[(int)ResourceTypeEnum.Spice] = createResource(ResourceTypeEnum.Spice, "Spice", 10, 5);
            ResourceTypes.Types[(int)ResourceTypeEnum.Platinum] = createResource(ResourceTypeEnum.Platinum, "Platinum", 0.5, 25);
        }

        private static ResourceType createResource(ResourceTypeEnum type, String name, double volPerUnit, Int64 defaultToProd)
        {
            ResourceType res = new ResourceType();
            res.Type = type;
            res.Name = name;
            res.VolumePerUnit = volPerUnit;
            res.DefaultTicksToProduce = defaultToProd;
            return res;
        }
    }
}
