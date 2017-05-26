using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap
{
    public class GoapNode
    {
        private static int MaxID;
        public int ID;
        public GoapAction action;
        public GoapNode parent;
        public float runningCost;
        public Dictionary<string, object> state;
        public Dictionary<Int64, Int64> resources;
        public float weight;

        public GoapNode(GoapNode parent, float runningCost, float weight, Dictionary<string, object> state, Dictionary<Int64, Int64> resources, GoapAction action)
        {
            ID = MaxID++;
            ReInit(parent, runningCost, weight, state, resources, action);
        }

        public void ReInit(GoapNode parent, float runningCost, float weight, Dictionary<string, object> state, Dictionary<Int64, Int64> resources, GoapAction action)
        {
            Clear();
            this.parent = parent;
            this.runningCost = runningCost;
            this.weight = weight;
            this.state = state;
            this.resources = resources;
            this.action = action;
        }

        private void Clear()
        {
            this.parent = null;
            this.runningCost = 0;
            this.weight = 0;
            this.state = null;
            this.resources = null;
            this.action = null;
        }

        /// <summary>
        ///     compare node
        /// </summary>
        /// <param name="cheapest"></param>
        /// <returns></returns>
        public bool BetterThan(GoapNode rh)
        {
            return runningCost < rh.runningCost;
            //if (weight > rh.weight && runningCost < rh.runningCost)
            //    return true;
            //if (weight < rh.weight && runningCost > rh.runningCost)
            //    return false;
            ////make weight > cost
            //var better = (weight / rh.weight - 1) >= (runningCost / rh.runningCost - 1);
            //return better;
        }
    }
}
