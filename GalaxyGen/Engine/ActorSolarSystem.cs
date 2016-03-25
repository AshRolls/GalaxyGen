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
        SolarSystem _solarSystem;
        private List<IActorRef> _subscribedActorAgents;
        private List<IActorRef> _subscribedActorPlanets;

        public ActorSolarSystem(IActorRef actorTextOutput, SolarSystem ss)
        {
            _actorTextOutput = actorTextOutput;
            _solarSystem = ss;
            _solarSystem.Actor = Self;
            _subscribedActorAgents = new List<IActorRef>();
            _subscribedActorPlanets = new List<IActorRef>();

            // create child actors for each agent in ss
            foreach (Agent agent in _solarSystem.Agents)
            {
                Props agentProps = Props.Create<ActorAgent>(_actorTextOutput, agent, Self);
                IActorRef actor = Context.ActorOf(agentProps, "Agent" + agent.AgentId.ToString());
                _subscribedActorAgents.Add(actor);
            }

            // create child actors for each planet in ss
            foreach (Planet p in _solarSystem.Planets)
            {
                Props planetProps = Props.Create<ActorPlanet>(_actorTextOutput, p, Self);
                IActorRef actor = Context.ActorOf(planetProps, "Planet" + p.PlanetId.ToString());
                _subscribedActorPlanets.Add(actor);
            }

            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Solar System initialised : " + _solarSystem.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV SS: " + _solarSystemVm.Name + " " + tick.Tick.ToString());

            foreach (IActorRef humanActor in _subscribedActorAgents)
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
