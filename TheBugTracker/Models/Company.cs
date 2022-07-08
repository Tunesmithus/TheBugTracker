using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //Navigation Properties

        [NotMapped]
        public ICollection<BTUser> Members { get; set; }
        public ICollection<Project> Projects { get; set; }

        //create relationship to invites
        public ICollection<Invite> Invites { get; set; }

    }
}
