using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Framework;
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
        private SolarSystemController _solarSystemC;
        //private List<IActorRef> _subscribedActorAgents;
        private List<PlanetController> _planetCs;
        //private List<IActorRef> _subscribedActorShips;

        public ActorSolarSystem(IActorRef actorTextOutput, SolarSystem ss)
        {
            _actorTextOutput = actorTextOutput;
            ss.Actor = Self;
            _solarSystemC = new SolarSystemController(ss);
            _planetCs = new List<PlanetController>();

            // create child controller for each planet in ss
            foreach (Planet p in ss.Planets)
            {
                PlanetController pc = new PlanetController(p);
                _planetCs.Add(pc);
            }


            
            //// create child actors for each agent in ss
            //foreach (Agent agent in _solarSystem.Agents)
            //{
            //    Props agentProps = Props.Create<ActorAgent>(_actorTextOutput, agent, Self);
            //    IActorRef actor = Context.ActorOf(agentProps, "Agent" + agent.AgentId.ToString());
            //    _subscribedActorAgents.Add(actor);
            //}

            

            //// create child actors for each ship in ss
            //foreach (Ship s in _solarSystem.Ships)
            //{
            //    Props shipProps = Props.Create<ActorShip>(_actorTextOutput, s, Self);
            //    IActorRef actor = Context.ActorOf(shipProps, "Ship" + s.ShipId.ToString());
            //    _subscribedActorShips.Add(actor);
            //}

            Receive<MessageTick>(msg => receiveTick(msg));

           //_actorTextOutput.Tell("Solar System initialised : " + _solarSystem.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            _solarSystemC.Tick(tick);

            foreach (PlanetController pc in _planetCs)
            {
                pc.Tick(tick);
            }

            //foreach (IActorRef shipActor in _subscribedActorShips)
            //{
            //    shipActor.Tell(tick);
            //}

            //foreach (IActorRef agentActor in _subscribedActorAgents)
            //{
            //    agentActor.Tell(tick);
            //}            

        }


    }
}
