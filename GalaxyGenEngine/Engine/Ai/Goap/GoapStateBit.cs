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
        IsDocked = (1L << 1), // TODO move all bool flags into new flag ulong
        ShipStoreId = (1UL << 2),
        AllowedRes1 = (1UL << 3),
        AllowedRes2 = (1UL << 4),
        AllowedRes3 = (1UL << 5),
        AllowedRes4 = (1UL << 6),
        AllowedRes5 = (1UL << 7),        
        AllowedLoc1 = (1L << 8),
        AllowedLoc2 = (1L << 9),
        AllowedLoc3 = (1L << 10),
        AllowedLoc4 = (1L << 11),
        AllowedLoc5 = (1L << 12)
    }

    public record GoapStateResLoc(ResourceTypeEnum ResType, ulong StoreId);    

    public class GoapStateBit
    {
        public ulong Flags;
        public ulong DockedAt;
        public ulong IsDocked;
        public ulong ShipStoreId;
        public ulong AllowedRes1;
        public ulong AllowedRes2;
        public ulong AllowedRes3;
        public ulong AllowedRes4;
        public ulong AllowedRes5;
        public ulong AllowedLoc1;
        public ulong AllowedLoc2;
        public ulong AllowedLoc3;
        public ulong AllowedLoc4;
        public ulong AllowedLoc5;

        public ulong ResFlags;
        public Long64Array ResQtys = new Long64Array();     
       
        public ulong GetVal(GoapStateBitFlagsEnum bit)
        {
            switch (bit)
            {
                case GoapStateBitFlagsEnum.DockedAt: return DockedAt;
                case GoapStateBitFlagsEnum.IsDocked: return IsDocked;
                case GoapStateBitFlagsEnum.ShipStoreId: return ShipStoreId;
                case GoapStateBitFlagsEnum.AllowedRes1: return AllowedRes1;
                case GoapStateBitFlagsEnum.AllowedRes2: return AllowedRes2;
                case GoapStateBitFlagsEnum.AllowedRes3: return AllowedRes3;
                case GoapStateBitFlagsEnum.AllowedRes4: return AllowedRes4;
                case GoapStateBitFlagsEnum.AllowedRes5: return AllowedRes5;
                case GoapStateBitFlagsEnum.AllowedLoc1: return AllowedLoc1;
                case GoapStateBitFlagsEnum.AllowedLoc2: return AllowedLoc2;
                case GoapStateBitFlagsEnum.AllowedLoc3: return AllowedLoc3;
                case GoapStateBitFlagsEnum.AllowedLoc4: return AllowedLoc4;
                case GoapStateBitFlagsEnum.AllowedLoc5: return AllowedLoc5;
            }
            throw new Exception("Should not reach here");
        }

        public ulong GetVal(ulong valIdx)
        {
            return GetVal((GoapStateBitFlagsEnum)valIdx);            
        }   

        public ulong GetVal(int idx)
        {
            return GetVal((GoapStateBitFlagsEnum)(1UL << idx));
        }

        public void SetFlagAndVal(GoapStateBitFlagsEnum bit, ulong val)
        {
            setFlag(bit);
            setVal(bit, val);
        }

        private void setVal(GoapStateBitFlagsEnum bit, ulong val)
        {
            switch (bit)
            {
                case GoapStateBitFlagsEnum.DockedAt: DockedAt = val; break;
                case GoapStateBitFlagsEnum.IsDocked: IsDocked = val; break;
                case GoapStateBitFlagsEnum.ShipStoreId: ShipStoreId = val; break;
                case GoapStateBitFlagsEnum.AllowedRes1: AllowedRes1 = val; break;
                case GoapStateBitFlagsEnum.AllowedRes2: AllowedRes2 = val; break;
                case GoapStateBitFlagsEnum.AllowedRes3: AllowedRes3 = val; break;
                case GoapStateBitFlagsEnum.AllowedRes4: AllowedRes4 = val; break;
                case GoapStateBitFlagsEnum.AllowedRes5: AllowedRes5 = val; break;
                case GoapStateBitFlagsEnum.AllowedLoc1: AllowedLoc1 = val; break;
                case GoapStateBitFlagsEnum.AllowedLoc2: AllowedLoc2 = val; break;
                case GoapStateBitFlagsEnum.AllowedLoc3: AllowedLoc3 = val; break;
                case GoapStateBitFlagsEnum.AllowedLoc4: AllowedLoc4 = val; break;
                case GoapStateBitFlagsEnum.AllowedLoc5: AllowedLoc5 = val; break;
            }
        }

        private void setVal(int idx, ulong val)
        {
            setVal((GoapStateBitFlagsEnum)(1UL << idx), val);
        }                

        private void setFlag(ulong bit)
        {
            Flags |= bit;
        }

        private void setFlag(GoapStateBitFlagsEnum bit)
        {
            Flags |= (ulong)bit;
        }

        private void setFlag(int idx)
        {
            Flags |= (1UL << idx);
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
            setResFlag(idx);
            ResQtys.Values[idx] += qty;
        }        

        private void setResFlag(int bitIdx)
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
                    newGs.setFlag(i);
                    newGs.setVal(i, toApply.GetVal(i));
                }                    
            }
            
            for (int i = 0; i < resLocsCount; i++)
            {
                if (toApply.HasResFlag(i))
                {
                    newGs.SetResFlagAndVal(i, toApply.ResQtys.Values[i]);                    
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
                    newGs.setFlag(i);
                    newGs.setVal(i, this.GetVal(i));
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
                   IsDocked == bit.IsDocked &&
                   ShipStoreId == bit.ShipStoreId &&
                   AllowedRes1 == bit.AllowedRes1 &&
                   AllowedRes2 == bit.AllowedRes2 &&
                   AllowedRes3 == bit.AllowedRes3 &&
                   AllowedRes4 == bit.AllowedRes4 &&
                   AllowedRes5 == bit.AllowedRes5 &&
                   AllowedLoc1 == bit.AllowedLoc1 &&
                   AllowedLoc2 == bit.AllowedLoc2 &&
                   AllowedLoc3 == bit.AllowedLoc3 &&
                   AllowedLoc4 == bit.AllowedLoc4 &&
                   AllowedLoc5 == bit.AllowedLoc5 &&
                   ResFlags == bit.ResFlags &&
                   EqualityComparer<Long64Array>.Default.Equals(ResQtys, bit.ResQtys);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Flags);
            hash.Add(DockedAt);
            hash.Add(IsDocked);
            hash.Add(ShipStoreId);
            hash.Add(AllowedRes1);
            hash.Add(AllowedRes2);
            hash.Add(AllowedRes3);
            hash.Add(AllowedRes4);
            hash.Add(AllowedRes5);
            hash.Add(AllowedLoc1);
            hash.Add(AllowedLoc2);
            hash.Add(AllowedLoc3);
            hash.Add(AllowedLoc4);
            hash.Add(AllowedLoc5);
            hash.Add(ResFlags);
            hash.Add(ResQtys);
            return hash.ToHashCode();
        }

        internal void AddAllowedResources(GoapStateBit toApply)
        {
            ulong bitIdx = (ulong)GoapStateBitFlagsEnum.AllowedRes1;
            for (int i = 0; i < GoapPlanner.ALLOWED_RES_MAX; i++)  
            {                
                if (toApply.HasFlag(bitIdx))
                {
                    this.setFlag(bitIdx);
                    this.setVal((GoapStateBitFlagsEnum)bitIdx, toApply.GetVal(bitIdx));
                }
                bitIdx = bitIdx << 1;
            }
        }

        internal (bool allowed, (bool newAllowedRes, int allowedIdx)) IsAllowedResource(ResourceTypeEnum resType)
        {
            ulong bitIdx = (ulong)GoapStateBitFlagsEnum.AllowedRes1;
            for (int i = 0; i < GoapPlanner.ALLOWED_RES_MAX; i++)
            {
                if (HasFlag(bitIdx))
                {
                    if (GetVal(bitIdx) == (ulong)resType) return (true, (false, i));
                }
                else return (true, (true, i));
                
                bitIdx = bitIdx << 1;
            }
            return (false, (false,0));
        }

        internal void AddAllowedDestinations(GoapStateBit toApply)
        {
            ulong bitIdx = (ulong)GoapStateBitFlagsEnum.AllowedLoc1;
            for (int i = 0; i < GoapPlanner.ALLOWED_DEST_MAX; i++)
            {
                if (toApply.HasFlag(bitIdx))
                {
                    this.setFlag(bitIdx);
                    this.setVal((GoapStateBitFlagsEnum)bitIdx, toApply.GetVal(bitIdx));
                }
                bitIdx = bitIdx << 1;
            }
        }

        internal (bool allowed, (bool newAllowedDest, int allowedIdx)) IsAllowedDestination(ulong dest)
        {
            ulong bitIdx = (ulong)GoapStateBitFlagsEnum.AllowedLoc1;
            for (int i = 0; i < GoapPlanner.ALLOWED_DEST_MAX; i++)
            {
                if (HasFlag(bitIdx))
                {
                    if (GetVal(bitIdx) == dest) return (true, (false, i));
                }
                else return (true, (true, i));

                bitIdx = bitIdx << 1;
            }
            return (false, (false, 0));
        }

    }
}
