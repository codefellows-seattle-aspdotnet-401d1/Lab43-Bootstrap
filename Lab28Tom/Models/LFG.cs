using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab28Tom.Models
{
    public class LFG
    {
        public int ID { get; set; }

        [Display(Name = "Gamertag/PSN ID")]
        public string Gamertag { get; set; }

        [Display(Name = "Class")]
        public string DestinyClass { get; set; }

        [Display(Name = "Notes")]
        public string Request { get; set; }
    }
}
