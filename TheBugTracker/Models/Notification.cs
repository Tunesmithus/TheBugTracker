﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Message")]
        [Required]
        public string Message { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset Created { get; set; }

        [Required]
        [DisplayName("Recipient")]
        public string RecipientId { get; set; }

        [Required]
        [DisplayName("Sender")]
        public string SenderId { get; set; }

        [DisplayName("Has Been Viewed")]
        public bool Viewed { get; set; }


        //Navigation Properties//
        public virtual Ticket Ticket { get; set; }
        public virtual BTUser Sender { get; set; }
        public virtual BTUser Recipient { get; set; }

    }
}
