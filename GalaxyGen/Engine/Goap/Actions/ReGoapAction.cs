using GalaxyGen.Engine.Goap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Goap.Actions
{
    public abstract class ReGoapAction<T, W> : IReGoapAction<T,W>
    {
        public string Name = "GoapAction";

        protected ReGoapState<T, W> preconditions;
        protected ReGoapState<T, W> effects;
        public float Cost = 1;

        protected IReGoapAgent<T, W> agent;
        protected Dictionary<string, object> genericValues;
        protected bool interruptWhenPossible;

        protected IReGoapActionSettings<T, W> settings = null;

        private bool inRange = false;

        public ReGoapAction()
        {
            effects = ReGoapState<T, W>.Instantiate();
            preconditions = ReGoapState<T, W>.Instantiate();

            genericValues = new Dictionary<string, object>();
        }

        public virtual void PostPlanCalculations(IReGoapAgent<T, W> goapAgent)
        {
            agent = goapAgent;
        }

        public virtual bool IsInterruptable()
        {
            return true;
        }

        public virtual void AskForInterruption()
        {
            interruptWhenPossible = true;
        }

        public virtual void Precalculations(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState)
        {
            agent = goapAgent;
        }

        public virtual IReGoapActionSettings<T, W> GetSettings(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState)
        {
            return settings;
        }

        public virtual ReGoapState<T, W> GetPreconditions(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
        {
            return preconditions;
        }

        public virtual ReGoapState<T, W> GetEffects(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
        {
            return effects;
        }

        public virtual float GetCost(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
        {
            return Cost;
        }

        public virtual bool CheckProceduralCondition(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
        {
            return true;
        }

        public virtual bool Perform(IReGoapActionSettings<T, W> settings, ReGoapState<T, W> goalState)
        {
            interruptWhenPossible = false;
            this.settings = settings;
            return true;
        }

        public virtual Dictionary<string, object> GetGenericValues()
        {
            return genericValues;
        }

        public virtual string GetName()
        {
            return Name;
        }

        public override string ToString()
        {
            return string.Format("GoapAction('{0}')", Name);
        }

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

        /**
        * Does this action need to be within range of a target game object?
        * If not then the moveTo state will not need to run for this action.
        */
        public abstract bool requiresInRange();

        /**
        * Is the action done?
        */
        public abstract bool isDone();

        /**
        * Reset any variables that need to be reset before planning happens again.
        */
        public abstract void reset();

        /**
       * An action often has to perform on an object. This is that object. Can be null. */
        public object target { get; set; }


    }
}

