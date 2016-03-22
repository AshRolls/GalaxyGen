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
    }

    public class ResourceQuantity
    {
        public ResourceTypeEnum Type { get; set; }
        public Int64 Quantity { get; set; }
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

        public static BluePrint GetResource(BluePrintEnum resType)
        {
            int resIdx = (int)resType;            
            return types_Var[resIdx];
        }
    }

    public class BluePrintInitialiser
    {                        
        public BluePrintInitialiser()
        {
            // pull these in from XML eventually
            ResourceQuantity prod = new ResourceQuantity();
            prod.Type = ResourceTypeEnum.Platinum;
            prod.Quantity = 10;
            ResourceQuantity cons = new ResourceQuantity();
            cons.Type = ResourceTypeEnum.Spice;
            prod.Quantity = 100;
            BluePrints.Types[(int)BluePrintEnum.SpiceToPlatinum] = getBluePrint(BluePrintEnum.SpiceToPlatinum, "Spice To Platinum", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons });


            prod = new ResourceQuantity();
            prod.Type = ResourceTypeEnum.Spice;
            prod.Quantity = 10;
            cons = new ResourceQuantity();
            cons.Type = ResourceTypeEnum.Platinum;
            prod.Quantity = 1;
            BluePrints.Types[(int)BluePrintEnum.PlatinumToSpice] = getBluePrint(BluePrintEnum.PlatinumToSpice, "Platinum to Spice", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons });
        }

        private BluePrint getBluePrint(BluePrintEnum type, String name, List<ResourceQuantity> produces, List<ResourceQuantity> consumes)
        {
            BluePrint res = new BluePrint();
            res.Type = type;
            res.Name = name;
            res.Produces = produces;
            res.Consumes = consumes;
            return res;
        }
    }
}
