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

    public record struct ResourceQuantityLocation(ResourceTypeEnum Type, ulong StoreId, long Qty);

    public record struct ResourceQuantity(ResourceTypeEnum Type, long Quantity);

    public record class ResourceType(ResourceTypeEnum Type, String Name, Double VolumePerUnit, Int64 DefaultTicksToProduce);

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
            ResourceTypes.Types[(int)ResourceTypeEnum.Exotic_Spice] = createResource(ResourceTypeEnum.Exotic_Spice, "Spice", 1, 5);
            ResourceTypes.Types[(int)ResourceTypeEnum.Metal_Platinum] = createResource(ResourceTypeEnum.Metal_Platinum, "Platinum", 3, 25);
            ResourceTypes.Types[(int)ResourceTypeEnum.Metal_Uranium] = createResource(ResourceTypeEnum.Metal_Uranium, "Uranium", 2, 15);
        }

        private static ResourceType createResource(ResourceTypeEnum type, String name, double volPerUnit, Int64 defaultToProd)
        {
            return new(type, name, volPerUnit, defaultToProd );            
        }
    }
}
