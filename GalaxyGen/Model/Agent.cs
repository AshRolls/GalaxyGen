using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Agent
    {
        [Key]
        public Int64 AgentId { get; set; }

        [Required]
        public String Name { get; set; }
    }
}
