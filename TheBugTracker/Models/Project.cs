﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        public int? CompanyId { get; set; }


        public int? ProjectPriorityId { get; set; }

        [DisplayName("Project Name")]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset StartDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset EndDate { get; set; }

        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile ImageFormFile { get; set; }

        [DisplayName("Image")]

        public byte[] ImageFileData { get; set; }

        public string ImageFileName { get; set; }

        [DisplayName("File Extension")]

        public string ImageContentType { get; set; }

        [DisplayName("Archived")]
        public bool Archived { get; set; }


        //Navigation Properties
        public Company Company { get; set; }

        [DisplayName("Priority")]

        public ProjectPriority ProjectPriority { get; set; }
        public ICollection<BTUser> Members {get; set;}
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
