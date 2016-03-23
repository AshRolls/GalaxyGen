using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public enum BluePrintEnum
    {
        NotSet = 0,
        SpiceToPlatinum = 1,
        PlatinumToSpice = 2
    }

    public class BluePrint
    {
        public BluePrintEnum Type { get; set; }
        public String Name { get; set; }
        public List<ResourceQuantity> Produces { get; set; }
        public List<ResourceQuantity> Consumes { get; set; }
        public int BaseTicks { get; set; }
    }

    public static class BluePrints
    {
        private static BluePrint[] types_Var;
        public static BluePrint[] Types
        {
            get
            {
                if (types_Var == null) types_Var = new BluePrint[Enum.GetNames(typeof(BluePrintEnum)).Length]; // initialise to size of enum
                return types_Var;
            }
            set
            {
                types_Var = value;
            }
        }

        public static BluePrint GetBluePrint(BluePrintEnum bpType)
        {
            int bpIdx = (int)bpType;            
            return types_Var[bpIdx];
        }
    }

    public class BluePrintInitialiser
    {                        
        public BluePrintInitialiser()
        {
            // pull these in from XML eventually
            ResourceQuantity prod = new ResourceQuantity();
            prod.Type = ResourceTypeEnum.Platinum;
            prod.Quantity = 1;
            ResourceQuantity cons = new ResourceQuantity();
            cons.Type = ResourceTypeEnum.Spice;
            cons.Quantity = 10;
            BluePrints.Types[(int)BluePrintEnum.SpiceToPlatinum] = getBluePrint(BluePrintEnum.SpiceToPlatinum, "Spice To Platinum", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons }, 10);


            prod = new ResourceQuantity();
            prod.Type = ResourceTypeEnum.Spice;
            prod.Quantity = 10;
            cons = new ResourceQuantity();
            cons.Type = ResourceTypeEnum.Platinum;
            cons.Quantity = 1;
            BluePrints.Types[(int)BluePrintEnum.PlatinumToSpice] = getBluePrint(BluePrintEnum.PlatinumToSpice, "Platinum to Spice", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons }, 50);
        }

        private BluePrint getBluePrint(BluePrintEnum type, String name, List<ResourceQuantity> produces, List<ResourceQuantity> consumes, int baseTicks)
        {
            BluePrint bp = new BluePrint();
            bp.Type = type;
            bp.Name = name;
            bp.Produces = produces;
            bp.Consumes = consumes;
            bp.BaseTicks = baseTicks;
            return bp;
        }
    }
}
