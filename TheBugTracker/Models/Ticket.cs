﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [StringLength(50)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created")]
        public DateTimeOffset Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Updated")]
        public DateTimeOffset? Updated { get; set; }

        [DisplayName("Archived")]
        public bool Archived { get; set; }

        [DisplayName("Archived By Project")]
        public bool ArchivedByProject { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; }

        [DisplayName("Ticket Type")]
        public int TicketTypeId { get; set; }

        [DisplayName("Ticket Priority")]
        public int TicketPriorityId { get; set; }

        [DisplayName("Ticket Status")]
        public int TicketStatusId { get; set; }

        [DisplayName("Ticket Owner")]

        public string OwnerUserId { get; set; }

        [DisplayName("Ticket Developer")]

        public string DeveloperUserId { get; set; }

        //Navigation Properties

        public virtual Project Project { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketType TicketType { get; set; }

        [DisplayName("Owner")]

        public virtual BTUser OwnerUser { get; set; }

        [DisplayName("Developer")]

        public virtual BTUser DeveloperUser { get; set; }

        //Collection
        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();

       
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();

    }
}
