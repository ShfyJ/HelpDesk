using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ITHelpDesk.Models
{
    public class Users : IdentityUser
    {
        [Required]
        public string FName { get; set; }
        public string LName { get; set; }
        public string Fullname
        {
            get { return FName + " " + LName; }
        }
        //#nullable enable
        
        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        [JsonIgnore]
        public  Address Address { get; set; }
        #nullable disable
        public virtual Manager Managers { get; set; }
        public virtual RequestMakers RequestMakers { get; set; }
        public virtual Workers Workers { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
