using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Models
{
    public class ProjectPriority
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Priority Name")]
        public string Name { get; set; }
    }
}
