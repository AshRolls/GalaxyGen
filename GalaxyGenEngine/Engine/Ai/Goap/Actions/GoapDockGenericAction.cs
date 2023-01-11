using Akka.Routing;
using GalaxyGenEngine.Engine.Ai.Goap;
using GalaxyGenEngine.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockGenericAction : GoapAction
    {
        private bool _requestSent = false;        

        public GoapDockGenericAction(HashSet<ulong> possibleDocks)
        {
            _targets = possibleDocks;

            GoapStateKey key = new GoapStateKey(GoapStateKeyTypeEnum.StateName,GoapStateKeyStateNameEnum.IsDocked,new GoapStateKeyResLoc());                        
            addPrecondition(key, false);            
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
            return true; 
        }
       
        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool isSpecific()
        {
            return false;
        }  

        public override List<GoapAction> GetSpecificActions(object agent, GoapState state)
        {
            List<GoapAction> actions = new List<GoapAction>();
            foreach (ulong t in _targets)
            {
                GoapDockSpecificAction a = new GoapDockSpecificAction(t);
                actions.Add(a);
            }
            return actions;
        }

        public override bool perform(object agent)
        {            
            return true;
        }

        private static HashSet<ulong> _targets;
        public static HashSet<ulong> Targets
        {
            get { return _targets; }
        }
    }
}
