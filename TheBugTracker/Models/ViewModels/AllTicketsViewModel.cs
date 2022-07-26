using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models.ViewModels
{
    public class AllTicketsViewModel
    {
        public Ticket Ticket { get; set; }

        public List<Ticket> AllTickets { get; set; }



    }
}
