using GalaxyGenCore.Resources;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapCreateBuyOrderGenericAction : GoapAction
    {       
        public GoapCreateBuyOrderGenericAction()
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
                for (int i = 0; i < ResourceTypes.Types.Length; i++) // TODO this is going to explode the tree, perhaps only goal resource and n random resources?
                {            
                    (bool allowed, (bool newAllowedRes, int allowedIdx) allowedStatus) = state.IsAllowedResource(ResourceTypes.Types[i].Type);
                    if (allowed)
                    {
                        actions.Add(new GoapCreateBuyOrderSpecificAction(new ResourceQuantity(ResourceTypes.Types[i].Type, 1L), dockedAtStoreId, dockedAt, allowedStatus, planner));
                        actions.Add(new GoapCreateBuyOrderSpecificAction(new ResourceQuantity(ResourceTypes.Types[i].Type, 2L), dockedAtStoreId, dockedAt, allowedStatus, planner));
                        actions.Add(new GoapCreateBuyOrderSpecificAction(new ResourceQuantity(ResourceTypes.Types[i].Type, 4L), dockedAtStoreId, dockedAt, allowedStatus, planner));
                        actions.Add(new GoapCreateBuyOrderSpecificAction(new ResourceQuantity(ResourceTypes.Types[i].Type, 8L), dockedAtStoreId, dockedAt, allowedStatus, planner));
                        actions.Add(new GoapCreateBuyOrderSpecificAction(new ResourceQuantity(ResourceTypes.Types[i].Type, 16L), dockedAtStoreId, dockedAt, allowedStatus, planner));
                        actions.Add(new GoapCreateBuyOrderSpecificAction(new ResourceQuantity(ResourceTypes.Types[i].Type, 32L), dockedAtStoreId, dockedAt, allowedStatus, planner));
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
