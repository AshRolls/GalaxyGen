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


        public override bool Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, IReGoapActionSettings<string, object> settings, ReGoapState<string, object> goalState)
        {
            base.Run(previous, next, settings, goalState);
            this.settings = (LoadResourceSettings)settings;
            ResourceQuantity _resourceQ = new ResourceQuantity(ResourceTypeEnum.Spice,1);
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
    }
}
