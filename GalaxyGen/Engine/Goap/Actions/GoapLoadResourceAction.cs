using GalaxyGen.Engine.Goap.Core;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Goap.Actions
{
    public class GoapLoadResourceAction : ReGoapAction<string, object>
    {
        public override IReGoapActionSettings<string, object> GetSettings(IReGoapAgent<string, object> goapAgent, ReGoapState<string, object> goalState)
        {
            settings = null;
            foreach (var pair in goalState.GetValues())
            {
                if (pair.Key.StartsWith("loadedResource"))
                {
                    var resourceName = pair.Key.Substring(14);
                    settings = new LoadResourceSettings
                    {
                        ResourceName = resourceName
                    };
                    break;
                }
            }
            return settings;
        }

        private bool _loaded = false;
        private bool _requestSent = false;
        private Int64 _target;

        public override void reset()
        {
            _loaded = false;
            _requestSent = false;            
        }

        public override bool isDone()
        {
            return _loaded == true;
        }


        public override bool requiresInRange()
        {
            return true;
        }

        public override ReGoapState<string, object> GetEffects(ReGoapState<string, object> goalState, IReGoapAction<string, object> next = null)
        {
            effects.Clear();

            foreach (var pair in goalState.GetValues())
            {
                if (pair.Key.StartsWith("loadedResource"))
                {
                    var resourceName = pair.Key.Substring(14);
                    effects.Set("loadedResource" + resourceName, true);
                    break;
                }
            }

            return effects;
        }

        public override ReGoapState<string, object> GetPreconditions(ReGoapState<string, object> goalState, IReGoapAction<string, object> next = null)
        {
            preconditions.Clear();

            foreach (var pair in goalState.GetValues())
            {
                if (pair.Key.StartsWith("loadedResource"))
                {
                    var resourceName = pair.Key.Substring(14);
                    preconditions.Set("hasResource" + resourceName, true);
                    break;
                }
            }

            return preconditions;
        }


        public override bool Perform(IReGoapActionSettings<string, object> settings, ReGoapState<string, object> goalState)
        {
            base.Perform(settings, goalState);
            this.settings = (LoadResourceSettings)settings;
            ResourceQuantity _resourceQ = new ResourceQuantity(ResourceTypeEnum.Spice,1);
            _loaded = true;
            agent.RequestLoadShip(_resourceQ);            
            return true;            
        }

        public override string ToString()
        {
            return string.Format("GoapAction('{0}')", Name);
        }
    }

    public class LoadResourceSettings : IReGoapActionSettings<string, object>
    {
        public string ResourceName;
        public Int64 targetScId;
    }
}
