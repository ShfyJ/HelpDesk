using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITHelpDesk.Models
{
    public partial class Address
    {
        public Address()
        {
            Request = new HashSet<Request>();
            Users = new HashSet<Users>();
        }

        [Key]
        public int AddressId { get; set; }
        [Required]
        public string Building { get; set; }
        #nullable enable
        public string? Block { get; set; }
        public string Full
        {
            get
            {
                return Building + " блок " + Block;
            }
        }
        public string Flag { get; set; }

        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
