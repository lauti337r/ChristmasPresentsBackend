using System;
using System.Collections.Generic;

#nullable disable

namespace ChristmasPresents.Model
{
    public partial class Kid
    {
        public int KidId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Area { get; set; }
        public string Note { get; set; }
        public string PictureUrl { get; set; }
        public byte? Hidden { get; set; }
        public virtual Present Present { get; set; }
    }
}
