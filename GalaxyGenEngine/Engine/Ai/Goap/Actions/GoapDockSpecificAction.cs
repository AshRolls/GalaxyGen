using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockSpecificAction : GoapAction
    {
        public GoapDockSpecificAction(ulong dockScId)
        {
            target = dockScId;
            GoapStateKey key = new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.IsDocked, new GoapStateKeyResLoc(), ResourceTypeEnum.NotSet);                                    
            addEffect(key, true);
            key.StateName = GoapStateKeyStateNameEnum.DockedAt;            
            addEffect(key, dockScId);
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return ag.StateProvider.CurrentShipIsDocked;
        }
    
        public override bool RequiresInRange()
        {
            return true; 
        }

        public override bool IsSpecific()
        {
            return true;
        }
        public override List<GoapAction> GetSpecificActions(object agent, GoapState state, GoapState goal)
        {
            return null;
        }

        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool Perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;           
            if (!ag.StateProvider.CurrentShipIsDocked) ag.ActionProvider.RequestDock();
            return true;
        }

    }
}
