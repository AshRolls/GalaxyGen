using GCEngine.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Model
{
    public class ShipType
    {
        public ShipType()
        {
            ShipTypeId = IdUtils.GetId();            
        }

        public Int64 ShipTypeId { get; set; }
        public String Name { get; set; }
        public Double MaxCruisingSpeedKmH { get; set; } // km per hour
    }
}
