﻿using GalaxyGenCore.Resources;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUnloadShipGenericAction : GoapAction
    {       
        public GoapUnloadShipGenericAction()
        {            
            AddPrecondition(GoapStateBitFlagsEnum.IsDocked, 1UL);
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            return true;
        }

        public override bool RequiresInRange()
        {
            return false;
        }
        public override bool IsSpecific()
        {
            return false;
        }
        
        public override List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal, GoapPlanner planner)
        {
            GoapAgent ag = (GoapAgent)agent;
            List<GoapAction> actions = new();

            ulong dockedAt = state.GetVal(GoapStateBitFlagsEnum.DockedAt);
            if (ag.StateProvider.TryGetPlanetStoreId(dockedAt, out ulong dockedAtStoreId))
            {
                ulong shipStoreId = state.GetVal(GoapStateBitFlagsEnum.ShipStoreId);

                for (int i = 0; i < planner.ResLocs.Count; i++)
                {
                    if (state.HasResFlag(i) && planner.ResLocsIdx[i].StoreId == shipStoreId)
                    {
                        (bool allowed, (bool newAllowedRes, int allowedIdx) allowedStatus) = state.IsAllowedResource(planner.ResLocsIdx[i].ResType);
                        if (allowed)
                        {
                            long qty = state.GetResVal(i);
                            if (qty > 0) actions.Add(new GoapUnloadShipSpecificAction(shipStoreId, dockedAtStoreId, new ResourceQuantity(planner.ResLocsIdx[i].ResType, 1L), allowedStatus, planner));
                            if (qty > 1) actions.Add(new GoapUnloadShipSpecificAction(shipStoreId, dockedAtStoreId, new ResourceQuantity(planner.ResLocsIdx[i].ResType, 2L), allowedStatus, planner));
                            if (qty > 3) actions.Add(new GoapUnloadShipSpecificAction(shipStoreId, dockedAtStoreId, new ResourceQuantity(planner.ResLocsIdx[i].ResType, 4L), allowedStatus, planner));
                            if (qty > 7) actions.Add(new GoapUnloadShipSpecificAction(shipStoreId, dockedAtStoreId, new ResourceQuantity(planner.ResLocsIdx[i].ResType, 8L), allowedStatus, planner));
                            if (qty > 15) actions.Add(new GoapUnloadShipSpecificAction(shipStoreId, dockedAtStoreId, new ResourceQuantity(planner.ResLocsIdx[i].ResType, 16L), allowedStatus, planner));
                            if (qty > 31) actions.Add(new GoapUnloadShipSpecificAction(shipStoreId, dockedAtStoreId, new ResourceQuantity(planner.ResLocsIdx[i].ResType, qty), allowedStatus, planner));
                        }
                    }
                }
            }
            return actions;
        }

        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool Perform(object agent)
        {
            return true;
        }
    }
}
