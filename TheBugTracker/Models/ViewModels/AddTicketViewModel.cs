using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models.ViewModels
{
    public class AddTicketViewModel
    {
        public Ticket Ticket { get; set; }
        public SelectList DeveloperList { get; set; }
        public SelectList TicketPriorityList { get; set; }
        public SelectList TicketStatusList { get; set; }
        public SelectList TicketTypeList { get; set; }



    }
}
