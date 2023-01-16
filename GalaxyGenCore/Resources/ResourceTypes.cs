using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenCore.Resources
{
    public enum ResourceTypeEnum : ulong
    {
        NotSet,
        Exotic_Spice,
        Metal_Platinum,
        Metal_Aluminium,
        Metal_Uranium,
        Gas_Xenon        
    }

    public struct ResourceQuantity
    {
        public ResourceQuantity(ResourceTypeEnum type, long qty)
        {
            Type = type;
            Quantity = qty;
        }

        public ResourceTypeEnum Type { get; set; }
        public long Quantity { get; set; }
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
                types_Var ??= new ResourceType[Enum.GetNames(typeof(ResourceTypeEnum)).Length]; // initialise to size of enum
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
            ResourceTypes.Types[(int)ResourceTypeEnum.Exotic_Spice] = createResource(ResourceTypeEnum.Exotic_Spice, "Spice", 10, 5);
            ResourceTypes.Types[(int)ResourceTypeEnum.Metal_Platinum] = createResource(ResourceTypeEnum.Metal_Platinum, "Platinum", 0.5, 25);
        }

        private static ResourceType createResource(ResourceTypeEnum type, String name, double volPerUnit, Int64 defaultToProd)
        {
            ResourceType res = new()
            {
                Type = type,
                Name = name,
                VolumePerUnit = volPerUnit,
                DefaultTicksToProduce = defaultToProd
            };
            return res;
        }
    }
}
