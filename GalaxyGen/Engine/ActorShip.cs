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
    //public class ActorShip : ReceiveActor, IWithUnboundedStash
    //{
    //    IActorRef _actorTextOutput;
    //    IActorRef _actorSolarSystem;
    //    Ship _ship;

    //    public ActorShip(IActorRef actorTextOutput, Ship ship, IActorRef actorSolarSystem)
    //    {
    //        _actorTextOutput = actorTextOutput;
    //        _actorSolarSystem = actorSolarSystem;
    //        _ship = ship;
    //        _ship.Actor = Self;

    //        if (ship.ShipState == ShipStateEnum.Docked) Docked();
    //        else Cruising();

    //        _actorTextOutput.Tell("Ship initialised : " + shipStatus());
    //    }

    //    private string shipStatus()
    //    {
    //        StringBuilder initStr = new StringBuilder();
    //        initStr.Append(_ship.Name);
    //        initStr.Append(" [");
    //        if (_ship.ShipState == ShipStateEnum.Docked)
    //        {
    //            initStr.Append(_ship.DockedPlanet.Name);
    //        }
    //        else
    //        {
    //            initStr.Append(_ship.PositionX);
    //            initStr.Append(",");
    //            initStr.Append(_ship.PositionY);
    //        }
    //        initStr.Append("]");
    //        return initStr.ToString();
    //    }

    //    private void Docked()
    //    {
    //        Receive<MessageTick>(msg => receiveDockedTick(msg));
    //        Receive<MessageShipCommand>(msg => receiveDockCommand(msg));
    //    }

    //    private void Cruising()
    //    {
    //        Receive<MessageTick>(msg => receiveCruisingTick(msg));
    //    }

    //    private void AwaitingUndockingResponse()
    //    {
    //        Receive<MessageTick>(msg => receiveAwaitingTick(msg));
    //        Receive<MessageShipResponse>(msg => receiveUndockResponse(msg));
    //    }

    //    private void receiveUndockResponse(MessageShipResponse msg)
    //    {
    //        if (msg.Response == true)
    //        {                
    //            Become(Cruising);
    //            _ship.Pilot.Actor.Tell(msg);
    //            _ship.PositionX = _ship.DockedPlanet.PositionX;
    //            _ship.PositionY = _ship.DockedPlanet.PositionY;
    //            _ship.DockedPlanet = null;
    //            _ship.ShipState = ShipStateEnum.Cruising;
    //        }
    //        else
    //        {
    //            Become(Docked);
    //        }
    //        Stash.UnstashAll();
    //    }

    //    public IStash Stash { get; set; }

    //    // TODO put in system for when we never receive a response!
    //    private void receiveAwaitingTick(MessageTick tick)
    //    {
    //        Stash.Stash(); // stash messages while we are waiting for our response.
    //    }

    //    private void receiveDockedTick(MessageTick tick)
    //    {            
    //        _actorTextOutput.Tell(shipStatus());
    //    }

    //    private void receiveDockCommand(MessageShipCommand cmd)
    //    {
    //        if (cmd.Command == ShipCommandEnum.Undock && _ship.ShipState == ShipStateEnum.Docked)
    //        {
    //            MessageShipCommand newCmd = new MessageShipCommand(cmd.Command, cmd.TickSent, _ship.ShipId);
    //            _actorSolarSystem.Tell(newCmd);
    //            Become(AwaitingUndockingResponse);
    //        }
    //    }

    //    private void receiveCruisingTick(MessageTick tick)
    //    {            
    //        _actorTextOutput.Tell(shipStatus());
    //    }

    //}
}
