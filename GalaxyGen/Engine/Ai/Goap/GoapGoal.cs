using GalaxyGen.Engine.Goap.Core;
using GalaxyGen.Engine.Goap.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap
{
    public class GoapGoal<T, W> : IReGoapGoal<T,W>
    {
        public string Name = "GenericGoal";
        public float Priority = 1;
        public float ErrorDelay = 0.5f;

        protected ReGoapState<T, W> goal;
        protected Queue<ReGoapActionState<T, W>> plan;
        protected IGoapPlanner<T, W> planner;


        public GoapGoal()
        {
            goal = ReGoapState<T, W>.Instantiate();
        }

        protected virtual void OnDestroy()
        {
            goal.Recycle();
        }

        public virtual string GetName()
        {
            return Name;
        }

        public virtual float GetPriority()
        {
            return Priority;
        }

        public virtual bool IsGoalPossible()
        {
            return true;
        }

        public virtual Queue<ReGoapActionState<T, W>> GetPlan()
        {
            return plan;
        }

        public virtual ReGoapState<T, W> GetGoalState()
        {
            return goal;
        }

        public virtual void SetPlan(Queue<ReGoapActionState<T, W>> path)
        {
            plan = path;
        }

        public void Run(Action<IReGoapGoal<T, W>> callback)
        {
        }

        public virtual void Precalculations(IGoapPlanner<T, W> goapPlanner)
        {
            planner = goapPlanner;
        }

        public virtual float GetErrorDelay()
        {
            return ErrorDelay;
        }

        public static string PlanToString(IEnumerable<IReGoapAction<T, W>> plan)
        {
            var result = "GoapPlan(";
            var reGoapActions = plan as IReGoapAction<T, W>[] ?? plan.ToArray();
            var end = reGoapActions.Length;
            for (var index = 0; index < end; index++)
            {
                var action = reGoapActions[index];
                result += string.Format("'{0}'{1}", action, index + 1 < end ? ", " : "");
            }
            result += ")";
            return result;
        }

        public override string ToString()
        {
            return string.Format("GoapGoal('{0}')", Name);
        }
    }
}

