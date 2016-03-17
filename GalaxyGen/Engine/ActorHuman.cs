using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorHuman : ReceiveActor
    {        
        public ActorHuman()
        {
            Receive<ActorInitialiseMessage>(msg => receiveInitialiseMsg(msg));
        }

        private void receiveInitialiseMsg(ActorInitialiseMessage msg)
        {
            currentTick = msg.CurrentTick;
            agent = msg.Agent;
        }

        private Int64 currentTick
        {
            get; set;
        }

        private Agent agent
        {
            get; set;
        }

    }
}
