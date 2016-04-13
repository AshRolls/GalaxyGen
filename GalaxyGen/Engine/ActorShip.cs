﻿using Akka.Actor;
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
    public class ActorShip : ReceiveActor, IWithUnboundedStash
    {
        IActorRef _actorTextOutput;
        IActorRef _actorSolarSystem;
        Ship _ship;

        public ActorShip(IActorRef actorTextOutput, Ship ship, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _ship = ship;
            _ship.Actor = Self;

            if (ship.ShipState == ShipStateEnum.Docked) Docked();
            else Cruising();
                   
            _actorTextOutput.Tell("Ship initialised : " + _ship.Name);            
        }

        private void Docked()
        {
            Receive<MessageTick>(msg => receiveDockedTick(msg));
            Receive<MessageShipDockCommand>(msg => receiveDockCommand(msg));
        }

        private void Cruising()
        {
            Receive<MessageTick>(msg => receiveCruisingTick(msg));
        }

        private void AwaitingUndockingResponse()
        {
            Receive<MessageTick>(msg => receiveAwaitingTick(msg));
            Receive<MessageShipDockResponse>(msg => receiveUndockResponse(msg));
        }

        private void receiveUndockResponse(MessageShipDockResponse msg)
        {
            if (msg.Response == true)
            {                
                Become(Cruising);
                _ship.Pilot.Actor.Tell(msg);
            }
            else
            {
                Become(Docked);
            }
            Stash.UnstashAll();
        }

        public IStash Stash { get; set; }

        // TODO put in system for when we never receive a response!
        private void receiveAwaitingTick(MessageTick tick)
        {
            Stash.Stash(); // stash messages while we are waiting for our response.
        }

        private void receiveDockedTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV H: " + _agentVm.Name + " " + tick.Tick.ToString());
        }

        private void receiveDockCommand(MessageShipDockCommand cmd)
        {
            if (cmd.DockCommand == ShipDockCommandEnum.Undock && _ship.ShipState == ShipStateEnum.Docked)
            {
                MessageShipDockCommand newCmd = new MessageShipDockCommand(cmd.DockCommand, cmd.TickSent, _ship);
                _ship.DockedPlanet.Actor.Tell(newCmd);
                Become(AwaitingUndockingResponse);
            }
        }

        private void receiveCruisingTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV H: " + _agentVm.Name + " " + tick.Tick.ToString());
        }

    }
}
