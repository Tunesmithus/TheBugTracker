using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models
{
    public class BTUserProject
    {
        public int Id { get; set; }
        public string MemberId { get; set; }

        public BTUser Member { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
