﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models
{
    public class Invite
    {
        public int Id { get; set; }

        public DateTimeOffset InviteDate { get; set; }

        public DateTimeOffset JoinDate { get; set; }

        public Guid CompanyToken { get; set; }

        public int CompanyId { get; set; }

        public int ProjectId { get; set; }

        public string InvitorId { get; set; }

        public string InviteeId { get; set; }

        public string InviteeEmail { get; set; }

        public string InviteeFirstName { get; set; }

        public string InviteeLastName { get; set; }

        public bool IsValid { get; set; }


        //Navigation Properties

        public Company Company { get; set; }
        public Project Project { get; set; }
        public BTUser Invitor { get; set; }
        public BTUser Invitee { get; set; }
        
    }
}
