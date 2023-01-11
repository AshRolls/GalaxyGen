using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenEngine.Engine.Ai.Goap;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapLoadShipGenericAction : GoapAction
    {
        private bool _requestSent = false;

        public GoapLoadShipGenericAction()
        {
            GoapStateKey key = new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.IsDocked, new GoapStateKeyResLoc());
            addPrecondition(key, true);
        }

        public override void reset()
        {
            _requestSent = false;
        }

        public override bool isDone(object agent)
        {
            return true;
        }

        public override bool requiresInRange()
        {
            return false;
        }
        public override bool isSpecific()
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
                    if (kvp.Key.Type == GoapStateKeyTypeEnum.Resource && kvp.Key.ResourceLocation.StoreId == dockedAtStoreId)
                    {
                        GoapLoadShipSpecificAction aSingle = new GoapLoadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, shipStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, 1L));
                        GoapLoadShipSpecificAction aAll = new GoapLoadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, shipStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, (long)kvp.Value));
                        actions.Add(aSingle);
                        actions.Add(aAll);
                    }
                }
            }
            return actions;
        }

        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool perform(object agent)
        {
            return true;
        }
    }
}
