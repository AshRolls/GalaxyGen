using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public enum ShipDockCommandEnum
    {
        Undock,
        Dock
    }

    public class MessageShipDockCommand : Message
    {
        public MessageShipDockCommand(ShipDockCommandEnum dockCmd, Int64 tickSent, Ship ship)
        {
            DockCommand = dockCmd;
            TickSent = tickSent;
            Ship = ship;
        }

        public ShipDockCommandEnum DockCommand { get; private set; }        
        public Ship Ship { get; private set; }
    }
}
