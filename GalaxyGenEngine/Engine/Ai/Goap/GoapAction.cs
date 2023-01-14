
using GalaxyGenCore.Resources;
using GalaxyGenEngine.Engine.Ai.Goap;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    public abstract class GoapAction
    {
        private GoapStateBit _preconditions;
        private GoapStateBit _effects;     
        private bool _inRange = false;
   
        /* The cost of performing the action. 
         * Figure out a weight that suits the action. 
         * Changing it will affect what actions are chosen during planning.*/
        private float _cost = 1f;
        public virtual float GetCost()
        {
            return _cost;
        }

        // The risk of performing the action.
        public float Risk = 0f;
        // The Benefits of performing the action. 
        public float Return = 1f;
        // Figure out a weight that suits the action. 
        public virtual float GetWeight() => (1 - Risk) * Return;        
        // An action often has to perform on an object. This is that object. Can be null. 
        public object target;

        public GoapAction()
        {
            _preconditions = new GoapStateBit();
            _effects = new GoapStateBit();
        }

        public void DoReset()
        {
            _inRange = false;
            target = null;
            Reset();
        }

        /**
         * Reset any variables that need to be reset before planning happens again.
         */
        public abstract void Reset();

        /**
         * Is the action done?
         */
        public abstract bool IsDone(object agent);

        /**
         * Procedurally check if this action can run. Not all actions
         * will need this, but some might.
         */
        public abstract bool CheckProceduralPrecondition(object agent);

        /**
         * Run the action.
         * Returns True if the action performed successfully or false
         * if something happened and it can no longer perform. In this case
         * the action queue should clear out and the goal cannot be reached.
         */
        public abstract bool Perform(object agent);

        /**
         * Does this action need to be within range of a target game object?
         * If not then the moveTo state will not need to run for this action.
         */
        public abstract bool RequiresInRange();

        /**
         * Are we in range of the target?
         * The MoveTo state will set this and it gets reset each time this action is performed.
         */
        public bool isInRange()
        {
            return _inRange;
        }

        public void setInRange(bool inRange)
        {
            this._inRange = inRange;
        }

        public abstract bool IsSpecific();

        public abstract List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal, GoapPlanner planner);

        public void addPrecondition(GoapStateBitFlagsEnum flag, ulong value)
        {            
            _preconditions.SetFlagAndVal(flag, value);
        }

        public void addEffect(GoapStateBitFlagsEnum flag, ulong value)
        {            
            _effects.SetFlagAndVal(flag, value);
        }

        // TODO this might fail if try add resource fails after 64 res locs
        public void addResEffect(GoapStateResLoc resLoc, long qty, GoapPlanner planner)
        {   
            if (planner.TryAddResourceLocation(resLoc, out int idx)) _effects.SetResFlagAndVal(idx, qty);
            else _effects.SetResFlagAndVal(planner.GetResourceLocationIdx(resLoc), qty);            
        }

        public GoapStateBit Preconditions
        {
            get
            {
                return _preconditions;
            }
        }

        public GoapStateBit Effects
        {
            get
            {
                return _effects;
            }
        }

        public static string PrettyPrint(Queue<GoapAction> actions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (GoapAction a in actions)
            {
                sb.Append(a.GetType().Name);
                sb.Append("-> ");
            }
            sb.Append("GOAL");
            return sb.ToString();
        }

        public static string PrettyPrint(GoapAction[] actions)
        {
            StringBuilder sb = new StringBuilder();            
            foreach (GoapAction a in actions)
            {
                sb.Append(a.GetType().Name);
                sb.Append(", ");
            }
            return sb.ToString();
        }

        public static string PrettyPrint(GoapAction action)
        {
            return action.GetType().Name;
        }
    }
}