
using GalaxyGen.Engine.Goap.Core;
using System;
using System.Collections.Generic;

namespace GalaxyGen.Engine.Ai.Goap
{
    public abstract class GoapAction<T,W> : IReGoapAction<T,W>
    {
        private ReGoapState<T, W> preconditions;
        private ReGoapState<T, W> effects;
        //private Dictionary<Int64, Int64> resources;

        protected IReGoapActionSettings<T, W> settings = null;
        protected IReGoapAgent<T, W> agent;

        private bool _inRange = false;

        public Int64 TargetScId { get; private set; }

        /* The cost of performing the action. 
         * Figure out a weight that suits the action. 
         * Changing it will affect what actions are chosen during planning.*/
        private float _cost = 1f;
        public virtual float GetCost(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
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

        public GoapAction()
        {
            preconditions = ReGoapState<T, W>.Instantiate();
            effects = ReGoapState<T, W>.Instantiate();
            //resources = new Dictionary<Int64, Int64>();
        }

        public void doReset()
        {
            _inRange = false;
            TargetScId = 0;
            reset();
        }

        /**
         * Reset any variables that need to be reset before planning happens again.
         */
        public abstract void reset();

        /**
         * Is the action done?
         */
     
        public abstract bool checkProceduralPrecondition(object agent);

        public abstract bool RequiresInRange();
        public abstract bool IsDone();
        /**
         * Are we in range of the target?
         * The MoveTo state will set this and it gets reset each time this action is performed.
         */
        public bool IsInRange()
        {
            return _inRange;
        }

        public void SetInRange(bool inRange)
        {
            this._inRange = inRange;
        }


        public void addPrecondition(T key, W value)
        {
            preconditions.Set(key, value);
        }


        public void removePrecondition(T key)
        {
            if (preconditions.HasKey(key))
                preconditions.Remove(key);
        }

        public void addEffect(T key, W value)
        {
            effects.Set(key, value);
        }


        public void removeEffect(T key)
        {
            if (effects.HasKey(key))
                effects.Remove(key);
        }

        public virtual IReGoapActionSettings<T, W> GetSettings(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState)
        {
            return settings;
        }

        public abstract bool Perform(IReGoapActionSettings<T, W> settings, ReGoapState<T, W> goalState);

        public abstract string Name { get; }

        public ReGoapState<T, W> GetPreconditions(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
        {
            return preconditions;
        }

        public ReGoapState<T, W> GetEffects(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
        {
            return effects;
        }

        public virtual bool CheckProceduralCondition(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null)
        {
            return true;
        }

        public virtual void PostPlanCalculations(IReGoapAgent<T, W> goapAgent)
        {
            agent = goapAgent;
        }

        public virtual void Precalculations(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState)
        {
            agent = goapAgent;
        }
    }
}