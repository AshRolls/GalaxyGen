using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Agent : IAgent
    {
        [Key]
        public Int64 Id { get; set; }

        public String Name { get; set; }
    }
}
