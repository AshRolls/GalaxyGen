﻿using GalaxyGenCore.Resources;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUnloadShipGenericAction : GoapAction
    {
        static GoapStateKey dockedAtkey = new(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.IsDocked, new GoapStateKeyResLoc(), ResourceTypeEnum.NotSet);
        
        public GoapUnloadShipGenericAction()
        {            
            addPrecondition(dockedAtkey, true);
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
        
        public override List<GoapAction> GetSpecificActions(object agent, GoapState state, GoapState goal)
        {
            GoapAgent ag = (GoapAgent)agent;
            List<GoapAction> actions = new();
            List<ResourceTypeEnum> allowedResourceTypes = new(); // list faster than hashset for small sets
            foreach (KeyValuePair<GoapStateKey, object> kvp in goal.GetValues())
            {
                if (kvp.Key.Type == GoapStateKeyTypeEnum.ResourceQty && !allowedResourceTypes.Contains(kvp.Key.ResourceLocation.ResType)) allowedResourceTypes.Add(kvp.Key.ResourceLocation.ResType);
            }

            foreach (KeyValuePair<GoapStateKey, object> kvp in state.GetValues())
            {
                if (kvp.Key.Type == GoapStateKeyTypeEnum.AllowedResource && !allowedResourceTypes.Contains(kvp.Key.AllowedResource)) allowedResourceTypes.Add(kvp.Key.AllowedResource); 
            }

            ulong dockedAt = (ulong)state.Get(new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.DockedAt, new GoapStateKeyResLoc(), ResourceTypeEnum.NotSet));
            if (ag.StateProvider.TryGetPlanetStoreId(dockedAt, out ulong dockedAtStoreId))
            {
                ulong shipStoreId = (ulong)state.Get(new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.ShipStoreId, new GoapStateKeyResLoc(), ResourceTypeEnum.NotSet));
               
                foreach (KeyValuePair<GoapStateKey, object> kvp in state.GetValues())
                {
                    if (kvp.Key.Type == GoapStateKeyTypeEnum.ResourceQty && kvp.Key.ResourceLocation.StoreId == shipStoreId && allowedResourceTypes.Contains(kvp.Key.ResourceLocation.ResType))
                    {
                        actions.Add(new GoapUnloadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, dockedAtStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, 1L)));
                        if ((long)kvp.Value > 1) actions.Add(new GoapUnloadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, dockedAtStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, 2L)));
                        if ((long)kvp.Value > 3) actions.Add(new GoapUnloadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, dockedAtStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, 4L)));
                        if ((long)kvp.Value > 4) actions.Add(new GoapUnloadShipSpecificAction(kvp.Key.ResourceLocation.StoreId, dockedAtStoreId, new ResourceQuantity(kvp.Key.ResourceLocation.ResType, (long)kvp.Value)));
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
