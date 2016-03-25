using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public BluePrint(BluePrintEnum type, String name, List<ResourceQuantity> produces, List<ResourceQuantity> consumes, int baseTicks)
        {
            Type = type;
            Name = name;
            Produces = produces;
            Consumes = consumes;
            BaseTicks = baseTicks;
        }

        public BluePrintEnum Type { get; private set; }
        public String Name { get; private set; }
        public List<ResourceQuantity> Produces { get; private set; }
        public List<ResourceQuantity> Consumes { get; private set; }
        public int BaseTicks { get; set; }
    }

    public static class BluePrints
    {
        private static BluePrint[] types_Var = new BluePrint[Enum.GetNames(typeof(BluePrintEnum)).Length]; // initialise to size of enum
        private static ReadOnlyCollection<BluePrint> typesReadOnly_Var;
        public static ReadOnlyCollection<BluePrint> Types
        {
            get
            {
                if (typesReadOnly_Var == null)
                {                    
                    ReadOnlyCollection<BluePrint> typesReadOnly_Var = new ReadOnlyCollection<BluePrint>(types_Var);
                }
                return typesReadOnly_Var;
            }
            set
            {
                typesReadOnly_Var = value;
            }
        }

        public static BluePrint GetBluePrint(BluePrintEnum bpType)
        {
            int bpIdx = (int)bpType;            
            return types_Var[bpIdx];
        }

        public static void initialiseBluePrints()
        {
            // pull these in from XML eventually
            ResourceQuantity prod = new ResourceQuantity();
            prod.Type = ResourceTypeEnum.Platinum;
            prod.Quantity = 1;
            ResourceQuantity cons = new ResourceQuantity();
            cons.Type = ResourceTypeEnum.Spice;
            cons.Quantity = 10;
            types_Var[(int)BluePrintEnum.SpiceToPlatinum] = createBluePrint(BluePrintEnum.SpiceToPlatinum, "Spice To Platinum", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons }, 10);


            prod = new ResourceQuantity();
            prod.Type = ResourceTypeEnum.Spice;
            prod.Quantity = 10;
            cons = new ResourceQuantity();
            cons.Type = ResourceTypeEnum.Platinum;
            cons.Quantity = 1;
            types_Var[(int)BluePrintEnum.PlatinumToSpice] = createBluePrint(BluePrintEnum.PlatinumToSpice, "Platinum to Spice", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons }, 50);
        }

        private static BluePrint createBluePrint(BluePrintEnum type, String name, List<ResourceQuantity> produces, List<ResourceQuantity> consumes, int baseTicks)
        {
            BluePrint bp = new BluePrint(type, name, produces, consumes, baseTicks);
            return bp;
        }
    }   
}
