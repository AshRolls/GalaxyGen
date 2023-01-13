using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUnloadShipGenericAction : GoapAction
    {
        public GoapUnloadShipGenericAction()
        {
            GoapStateKey key = new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.IsDocked, new GoapStateKeyResLoc());
            addPrecondition(key, true);
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

        public override List<GoapAction> GetSpecificActions(object agent, GoapState state)
        {
            GoapAgent ag = (GoapAgent)agent;
            List<GoapAction> actions = new List<GoapAction>();
            
            ulong dockedAt = (ulong)state.Get(new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.DockedAt, new GoapStateKeyResLoc()));
            ulong dockedAtStoreId;
            if (ag.StateProvider.TryGetPlanetStoreId(dockedAt, out dockedAtStoreId))
            {
                ulong shipStoreId = (ulong)state.Get(new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.ShipStoreId, new GoapStateKeyResLoc()));
                foreach (KeyValuePair<GoapStateKey, object> kvp in state.GetValues())
                {
                    if (kvp.Key.Type == GoapStateKeyTypeEnum.Resource && kvp.Key.ResourceLocation.StoreId == shipStoreId)
                    {
                        actions.Add(new GoapUnloadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, dockedAtStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, 1L)));
                        if ((long)kvp.Value > 1) actions.Add(new GoapUnloadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, dockedAtStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, (long)kvp.Value)));                                                
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
