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

        public GoapDockGenericAction(HashSet<long> possibleDocks)
        {
            _targets = possibleDocks;

            GoapStateKey key = new GoapStateKey();            
            key.Type = GoapStateKeyEnum.String;
            key.String = "IsDocked";
            addPrecondition(key, false);            
        }

        public override void reset()
        {
            _requestSent = false;
        }

        public override bool isDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return ag.StateProvider.CurrentShipIsDocked;
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

        public override List<GoapAction> GetSpecificActions(object agent)
        {
            List<GoapAction> actions = new List<GoapAction>();
            foreach (long t in _targets)
            {
                GoapDockSpecificAction a = new GoapDockSpecificAction(t);
                actions.Add(a);
            }
            return actions;
        }

        public override bool perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
           
            if (!ag.StateProvider.CurrentShipIsDocked && !_requestSent)
            {
                ag.ActionProvider.RequestDock();
                _requestSent = true;
            }

            return true;
        }

        private static HashSet<long> _targets;
        public static HashSet<long> Targets
        {
            get { return _targets; }
        }
    }
}
