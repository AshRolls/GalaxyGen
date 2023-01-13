using Akka.Event;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap
{    
    [Flags]
    public enum GoapStateBitFlagsEnum : ulong
    {
        NotSet = 0,
        DockedAt = (1UL << 0),
        ShipStoreId = (1UL << 1),
        AllowedRes1 = (1UL << 2),
        AllowedRes2 = (1UL << 3),
        AllowedRes3 = (1UL << 4)
    }

    public record GoapStateResLoc
    {
        public ResourceTypeEnum ResType;
        public ulong StoreId; // TODO modify later to Int32 if memory is getting bloated

        public GoapStateResLoc(ResourceTypeEnum resType, ulong storeId)
        {
            ResType = resType;
            StoreId = storeId;
        }
    }

    public class GoapStateBit
    {
        public ulong Flags;
        public ulong DockedAt;
        public ulong ShipStoreId;
        public ulong AllowedRes1;
        public ulong AllowedRes2;
        public ulong AllowedRes3;
        public ulong ResFlags;
        public long[] ResQtys = new long[64];       
       
        public ulong GetVal(GoapStateBitFlagsEnum bit)
        {
            if (!HasFlag(bit)) return 0UL;
            switch (bit)
            {
                case GoapStateBitFlagsEnum.DockedAt: return DockedAt;                   
                case GoapStateBitFlagsEnum.ShipStoreId: return ShipStoreId;
                case GoapStateBitFlagsEnum.AllowedRes1: return AllowedRes1;
                case GoapStateBitFlagsEnum.AllowedRes2: return AllowedRes2;
                case GoapStateBitFlagsEnum.AllowedRes3: return AllowedRes3;
            }
            throw new Exception("Should not reach here");
        }

        public ulong GetVal(ulong valIdx)
        {
            if (!HasFlag(valIdx)) return 0UL;
            switch (valIdx)
            {
                case 1UL: return DockedAt;
                case 1UL << 1: return ShipStoreId;
                case 1UL << 2: return AllowedRes1;
                case 1UL << 3: return AllowedRes2;
                case 1UL << 4: return AllowedRes3;
            }
            throw new Exception("Should not reach here");
        }   

        public ulong GetVal(int idx)
        {
            return this.GetVal(1UL << idx);
        }
        
        public void SetVal(int idx, ulong val)
        {
            switch (idx)
            {
                case 1: DockedAt = val; break;
                case 2: ShipStoreId = val; break;
                case 3: AllowedRes1 = val; break;
                case 4: AllowedRes2 = val; break;
                case 5: AllowedRes3 = val; break;
            }
        }

        public void SetVal(GoapStateBitFlagsEnum bit, ulong val)
        {
            switch (bit)
            {
                case GoapStateBitFlagsEnum.DockedAt: DockedAt = val; break;
                case GoapStateBitFlagsEnum.ShipStoreId: ShipStoreId = val; break;
                case GoapStateBitFlagsEnum.AllowedRes1: AllowedRes1 = val; break;
                case GoapStateBitFlagsEnum.AllowedRes2: AllowedRes2 = val; break;
                case GoapStateBitFlagsEnum.AllowedRes3: AllowedRes3 = val; break;
            }
        }

        public void SetFlag(GoapStateBitFlagsEnum bit)
        {
            Flags |= (ulong)bit;
        }

        public void SetFlag(int bitIdx)
        {
            Flags |= (1UL << bitIdx);
        }

        public void UnsetFlag(GoapStateBitFlagsEnum bit)
        {
            Flags &= ~(ulong)bit;
        }
        // Works with "None" as well
        public bool HasFlag(GoapStateBitFlagsEnum bit)
        {
            return (Flags & (ulong)bit) == (ulong)bit;
        }

        public bool HasFlag(int bitIdx)
        {
            return (Flags & (1UL << bitIdx)) == (1UL << bitIdx);
        }

        public bool HasFlag(ulong valIdx)
        {
            return (Flags & valIdx) == valIdx;
        }

        public void ToggleFlag(GoapStateBitFlagsEnum bit)
        {
            Flags ^= (ulong)bit;
        }

        public long GetResLocQty(int idx)
        {
            return ResQtys[idx];
        }

        public void SetResQty(int idx, long qty)
        {
            ResQtys[idx] = qty;
        }        

        public void SetResFlag(int bitIdx)
        {
            ResFlags |= (1UL << bitIdx);
        }

        public void UnsetResFlag(int bitIdx)
        {
            ResFlags &= ~(1UL << bitIdx);
        }

        public bool HasResFlag(int bitIdx)
        {
            return (ResFlags & (1UL << bitIdx)) == (1UL << bitIdx);
        }

        public bool HasResFlag(ulong valIdx)
        {
            return (ResFlags & valIdx) == valIdx;
        }

        public bool InStateBit(GoapStateBit test, int resLocCount)
        {            
            ulong valIdx = 1UL;
            for(int i = 0; i < GoapPlanner.FLAGS_COUNT; i++)
            {                
                if (test.HasFlag(valIdx))
                {
                    if (!(this.HasFlag(valIdx) && this.GetVal(valIdx) == test.GetVal(valIdx))) return false;
                }
                valIdx = valIdx << 1;
            }

            for (int i = 0; i < resLocCount; i++)
            {
                if (this.GetResLocQty(i) != test.GetResLocQty(i)) return false;
            }
            return true;
        }

        internal GoapStateBit GetNewState(GoapStateBit toApply, Dictionary<GoapStateResLoc, int> resLocs)
        {
            GoapStateBit newGs = this.DeepClone();
            for (int i = 0; i < GoapPlanner.FLAGS_COUNT; i++)
            {
                if (toApply.HasFlag(i)) 
                {
                    newGs.SetFlag(i);
                    newGs.SetVal(i, toApply.GetVal(i));
                }                    
            }
            
            for (int i = 0; i < 64; i++)
            {
                if (toApply.HasResFlag(i))
                {
                    newGs.ResQtys[i] += toApply.ResQtys[i];
                }
            }
            return newGs;
        }

        internal GoapStateBit DeepClone()
        {
            GoapStateBit newGs = new();
            for (int i = 0; i < GoapPlanner.FLAGS_COUNT; i++)
            {
                if (this.HasFlag(i))
                {
                    newGs.SetFlag(i);
                    newGs.SetVal(i, this.GetVal(i));
                }                
            }

            newGs.ResQtys = (long[])this.ResQtys.Clone();
            return newGs;
        }
    }
}
