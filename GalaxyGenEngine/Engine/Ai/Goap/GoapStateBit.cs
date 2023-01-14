using GalaxyGenCore.Resources;
using GalaxyGenEngine.Framework;
using System;
using System.Collections.Generic;

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
        AllowedRes3 = (1UL << 4),
        IsDocked = (1L << 5) // TODO move all bool flags into new flag ulong
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
        public ulong IsDocked;

        public ulong ResFlags;
        public Long64Array ResQtys = new Long64Array();     
       
        public ulong GetVal(GoapStateBitFlagsEnum bit)
        {
            switch (bit)
            {
                case GoapStateBitFlagsEnum.DockedAt: return DockedAt;                   
                case GoapStateBitFlagsEnum.ShipStoreId: return ShipStoreId;
                case GoapStateBitFlagsEnum.AllowedRes1: return AllowedRes1;
                case GoapStateBitFlagsEnum.AllowedRes2: return AllowedRes2;
                case GoapStateBitFlagsEnum.AllowedRes3: return AllowedRes3;
                case GoapStateBitFlagsEnum.IsDocked: return IsDocked;
            }
            throw new Exception("Should not reach here");
        }

        public ulong GetVal(ulong valIdx)
        {
            switch (valIdx)
            {
                case 1UL: return DockedAt;
                case 1UL << 1: return ShipStoreId;
                case 1UL << 2: return AllowedRes1;
                case 1UL << 3: return AllowedRes2;
                case 1UL << 4: return AllowedRes3;
                case 1UL << 5: return IsDocked;
            }
            throw new Exception("Should not reach here");
        }   

        public ulong GetVal(int idx)
        {
            return this.GetVal(1UL << idx);
        }

        public void SetFlagAndVal(GoapStateBitFlagsEnum bit, ulong val)
        {
            SetFlag(bit);
            SetVal(bit, val);
        }

        public void SetVal(int idx, ulong val)
        {
            switch (idx)
            {
                case 0: DockedAt = val; break;
                case 1: ShipStoreId = val; break;
                case 2: AllowedRes1 = val; break;
                case 3: AllowedRes2 = val; break;
                case 4: AllowedRes3 = val; break;
                case 5: IsDocked = val; break;
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
                case GoapStateBitFlagsEnum.IsDocked: IsDocked = val; break;
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

        public long GetResVal(int idx)
        {
            return ResQtys.Values[idx];
        }

        //public bool SetResVal(GoapStateResLoc resLoc, long qty, GoapPlanner _planner)
        //{

        //}

        public void SetResFlagAndVal(int idx, long qty)
        {
            SetResFlag(idx);
            ResQtys.Values[idx] = qty;
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
            for (int i = 0; i < GoapPlanner.FLAGS_COUNT; i++)
            {                
                if (test.HasFlag(valIdx) && (!(this.HasFlag(valIdx) && this.GetVal(valIdx) == test.GetVal(valIdx)))) return false;                
                valIdx = valIdx << 1;
            }

            for (int i = 0; i < resLocCount; i++)
            {
                if (test.HasResFlag(i) && (this.GetResVal(i) != test.GetResVal(i))) return false;                
            }
            return true;
        }

        public int GetDifferenceCount(GoapStateBit other, int resLocCount)
        {
            int count = 0;
            ulong valIdx = 1UL;
            for (int i = 0; i < GoapPlanner.FLAGS_COUNT; i++)
            {
                if (other.HasFlag(valIdx) && (!(this.HasFlag(valIdx) && this.GetVal(valIdx) == other.GetVal(valIdx)))) count++;
                valIdx = valIdx << 1;
            }

            for (int i = 0; i < resLocCount; i++)
            {
                if (other.HasResFlag(i) && (this.GetResVal(i) != other.GetResVal(i))) count++;
            }
            return count;
        }

        internal GoapStateBit GetNewState(GoapStateBit toApply, int resLocsCount)
        {
            GoapStateBit newGs = this.DeepClone(resLocsCount);
            for (int i = 0; i < GoapPlanner.FLAGS_COUNT; i++)
            {
                if (toApply.HasFlag(i)) 
                {
                    newGs.SetFlag(i);
                    newGs.SetVal(i, toApply.GetVal(i));
                }                    
            }
            
            for (int i = 0; i < resLocsCount; i++)
            {
                if (toApply.HasResFlag(i))
                {
                    newGs.ResQtys.Values[i] += toApply.ResQtys.Values[i];
                }
            }
            return newGs;
        }

        internal GoapStateBit DeepClone(int resLocsCount)
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

            newGs.ResFlags = this.ResFlags;
            newGs.ResQtys.Values = (long[])this.ResQtys.Values.Clone();
            return newGs;
        }

        public override bool Equals(object obj)
        {
            return obj is GoapStateBit bit &&
                   Flags == bit.Flags &&
                   DockedAt == bit.DockedAt &&
                   ShipStoreId == bit.ShipStoreId &&
                   AllowedRes1 == bit.AllowedRes1 &&
                   AllowedRes2 == bit.AllowedRes2 &&
                   AllowedRes3 == bit.AllowedRes3 &&
                   IsDocked == bit.IsDocked &&
                   ResFlags == bit.ResFlags &&
                   EqualityComparer<Long64Array>.Default.Equals(ResQtys, bit.ResQtys);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Flags);
            hash.Add(DockedAt);
            hash.Add(ShipStoreId);
            hash.Add(AllowedRes1);
            hash.Add(AllowedRes2);
            hash.Add(AllowedRes3);
            hash.Add(IsDocked);
            hash.Add(ResFlags);
            hash.Add(ResQtys);
            return hash.ToHashCode();
        }
    }
}
