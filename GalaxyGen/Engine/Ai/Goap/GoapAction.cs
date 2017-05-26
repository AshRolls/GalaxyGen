
using System.Collections.Generic;

namespace GalaxyGen.Engine.Ai.Goap
{
    public abstract class GoapAction
    {
        private Dictionary<string, object> preconditions;
        private Dictionary<string, object> effects;

        private bool inRange = false;

        /* The cost of performing the action. 
         * Figure out a weight that suits the action. 
         * Changing it will affect what actions are chosen during planning.*/
        private float _cost = 1f;
        public virtual float GetCost()
        {
            return _cost;
        }

        /* The risk of performing the action. */
        public float Risk = 0f;
        /* The Benefits of performing the action. */
        public float Return = 1f;
        /* Figure out a weight that suits the action. */
        public virtual float GetWeight()
        {
            return (1 - Risk) * Return;
        }

        /**
         * An action often has to perform on an object. This is that object. Can be null. */
        public object target;

        public GoapAction()
        {
            preconditions = new Dictionary<string, object>();
            effects = new Dictionary<string, object>();
        }

        public void doReset()
        {
            inRange = false;
            target = null;
            reset();
        }

        /**
         * Reset any variables that need to be reset before planning happens again.
         */
        public abstract void reset();

        /**
         * Is the action done?
         */
        public abstract bool isDone();

        /**
         * Procedurally check if this action can run. Not all actions
         * will need this, but some might.
         */
        public abstract bool checkProceduralPrecondition(object agent);

        /**
         * Run the action.
         * Returns True if the action performed successfully or false
         * if something happened and it can no longer perform. In this case
         * the action queue should clear out and the goal cannot be reached.
         */
        public abstract bool perform(object agent);

        /**
         * Does this action need to be within range of a target game object?
         * If not then the moveTo state will not need to run for this action.
         */
        public abstract bool requiresInRange();


        /**
         * Are we in range of the target?
         * The MoveTo state will set this and it gets reset each time this action is performed.
         */
        public bool isInRange()
        {
            return inRange;
        }

        public void setInRange(bool inRange)
        {
            this.inRange = inRange;
        }


        public void addPrecondition(string key, object value)
        {
            preconditions.Add(key, value);
        }


        public void removePrecondition(string key)
        {
            if (preconditions.ContainsKey(key))
                preconditions.Remove(key);
        }


        public void addEffect(string key, object value)
        {
            effects.Add(key, value);
        }


        public void removeEffect(string key)
        {
            if (effects.ContainsKey(key))
                effects.Remove(key);
        }


        public Dictionary<string, object> Preconditions
        {
            get
            {
                return preconditions;
            }
        }

        public Dictionary<string, object> Effects
        {
            get
            {
                return effects;
            }
        }
    }
}