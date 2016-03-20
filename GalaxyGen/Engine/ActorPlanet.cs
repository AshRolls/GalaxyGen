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
    public class ActorPlanet : ReceiveActor
    {
        IActorRef _actorTextOutput;
        IPlanetViewModel _planetVm;
        IActorRef _actorSolarSystem;

        public ActorPlanet(IActorRef actorTextOutput, IPlanetViewModel planetVm, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _planetVm = planetVm;
            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Planet initialised : " + _planetVm.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            _actorTextOutput.Tell("TICK RCV P: " + _planetVm.Name + " " + tick.Tick.ToString());
        }

    }
}
