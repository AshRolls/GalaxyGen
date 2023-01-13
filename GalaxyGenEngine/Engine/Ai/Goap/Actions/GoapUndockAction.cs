using GalaxyGenCore.Resources;
using System.Collections.Generic;


namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUndockAction : GoapAction
    {
        public GoapUndockAction()
        {            
            GoapStateKey key = new(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.IsDocked, new GoapStateKeyResLoc(), ResourceTypeEnum.NotSet);
            addPrecondition(key, true);
            addEffect(key, false);
            key.StateName = GoapStateKeyStateNameEnum.DockedAt;
            addEffect(key, 0L);
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return !ag.StateProvider.CurrentShipIsDocked;
        }
    

        public override bool RequiresInRange()
        {
            return false; 
        }

        public override bool CheckProceduralPrecondition(object agent)
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

        public override bool Perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            if (ag.StateProvider.CurrentShipIsDocked) ag.ActionProvider.RequestUndock();
            return true;
        }
    }
}
