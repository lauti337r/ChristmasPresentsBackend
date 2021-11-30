using System;
using System.Collections.Generic;

#nullable disable

namespace ChristmasPresents.Model
{
    public partial class PresentGiver
    {
        public int PresentGiverId { get; set; }
        public string Name { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Letter { get; set; }
        public string PaymentMethod { get; set; }

        public virtual Present Present { get; set; }
    }
}
