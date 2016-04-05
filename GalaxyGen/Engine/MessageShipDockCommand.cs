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
        public MessageShipDockCommand(ShipDockCommandEnum dockCmd, Int64 tickSent)
        {
            DockCommand = dockCmd;
            TickSent = tickSent;
        }

        public ShipDockCommandEnum DockCommand { get; private set; }        
    }
}
