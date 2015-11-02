using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
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
    }

    public class ResourceTypeInitialiser
    {                        
        public ResourceTypeInitialiser()
        {
            // pull these in from XML eventually
            ResourceTypes.Types[(int)ResourceTypeEnum.Spice] = getResource(ResourceTypeEnum.Spice, "Spice", 10); 
            ResourceTypes.Types[(int)ResourceTypeEnum.Platinum] = getResource(ResourceTypeEnum.Platinum, "Platinum", 0.5);
        }

        private ResourceType getResource(ResourceTypeEnum type, String name, double volPerUnit)
        {
            ResourceType res = new ResourceType();
            res.Type = type;
            res.Name = name;
            res.VolumePerUnit = volPerUnit;
            return res;
        }
    }

    public enum ResourceTypeEnum
    {
        NotSet = 0,
        Spice = 1,
        Platinum = 2
    }

    public class ResourceType 
    {        
        public ResourceTypeEnum Type { get; set; }
        public String Name { get; set; }
        public Double VolumePerUnit { get; set; }
    }
}
