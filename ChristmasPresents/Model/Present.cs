using System;
using System.Collections.Generic;

#nullable disable

namespace ChristmasPresents.Model
{
    public partial class Present
    {
        public int PresentId { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public string ShopName { get; set; }
        public int? KidId { get; set; }
        public int? PresentGiverId { get; set; }

        public virtual Kid Kid { get; set; }
        public virtual PresentGiver PresentGiver { get; set; }
    }
}
