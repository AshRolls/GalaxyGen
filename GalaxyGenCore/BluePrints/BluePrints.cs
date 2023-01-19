using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GalaxyGenCore.BluePrints
{
    public enum BluePrintEnum
    {
        NotSet = 0,
        SpiceToPlatinum = 1,
        PlatinumToSpice = 2
    }

    public class BluePrint
    {
        public BluePrint(BluePrintEnum type, String name, List<ResourceQuantity> produces, List<ResourceQuantity> consumes, ulong baseTicks)
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
        public ulong BaseTicks { get; set; }
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
                    ReadOnlyCollection<BluePrint> typesReadOnly_Var = new(types_Var);
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

        public static void InitialiseBluePrints()
        {
            // pull these in from XML eventually
            ResourceQuantity prod = new ResourceQuantity(ResourceTypeEnum.Metal_Platinum, 2L);            
            ResourceQuantity cons = new ResourceQuantity(ResourceTypeEnum.Exotic_Spice, 2L);            
            types_Var[(int)BluePrintEnum.SpiceToPlatinum] = createBluePrint(BluePrintEnum.SpiceToPlatinum, "Spice To Platinum", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons }, 1333L);

            prod = new ResourceQuantity(ResourceTypeEnum.Exotic_Spice, 2L);
            cons = new ResourceQuantity(ResourceTypeEnum.Metal_Platinum, 2L);            
            types_Var[(int)BluePrintEnum.PlatinumToSpice] = createBluePrint(BluePrintEnum.PlatinumToSpice, "Platinum to Spice", new List<ResourceQuantity> { prod }, new List<ResourceQuantity> { cons }, 2000L);
        }

        private static BluePrint createBluePrint(BluePrintEnum type, String name, List<ResourceQuantity> produces, List<ResourceQuantity> consumes, ulong baseTicks)
        {
            BluePrint bp = new BluePrint(type, name, produces, consumes, baseTicks);
            return bp;
        }
    }
}
