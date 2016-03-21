using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorSolarSystem : ReceiveActor
    {
        IActorRef _actorTextOutput;
        ISolarSystemViewModel _solarSystemVm;
        private List<IActorRef> _subscribedActorHumans;
        private List<IActorRef> _subscribedActorPlanets;

        public ActorSolarSystem(IActorRef actorTextOutput, ISolarSystemViewModel ssVm)
        {
            _actorTextOutput = actorTextOutput;
            _solarSystemVm = ssVm;
            _subscribedActorHumans = new List<IActorRef>();
            _subscribedActorPlanets = new List<IActorRef>();

            // create child actors for each agent in ss
            foreach (IAgentViewModel agentVm in _solarSystemVm.Agents)
            {
                Props humanProps = Props.Create<ActorHuman>(_actorTextOutput, agentVm, Self);
                IActorRef actor = Context.ActorOf(humanProps, "Human" + agentVm.Model.AgentId.ToString());
                _subscribedActorHumans.Add(actor);
            }

            // create child actors for each planet in ss
            foreach (IPlanetViewModel planetVm in _solarSystemVm.Planets)
            {
                Props planetProps = Props.Create<ActorPlanet>(_actorTextOutput, planetVm, Self);
                IActorRef actor = Context.ActorOf(planetProps, "Planet" + planetVm.Model.PlanetId.ToString());
                _subscribedActorPlanets.Add(actor);
            }

            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Solar System initialised : " + _solarSystemVm.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            _actorTextOutput.Tell("TICK RCV SS: " + _solarSystemVm.Name + " " + tick.Tick.ToString());

            foreach (IActorRef humanActor in _subscribedActorHumans)
            {
                humanActor.Tell(tick);
            }

            foreach (IActorRef planetActor in _subscribedActorPlanets)
            {
                planetActor.Tell(tick);
            }

        }

    }
}
