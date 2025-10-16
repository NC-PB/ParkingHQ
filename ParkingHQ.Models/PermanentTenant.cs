using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class PermanentTenant
    {

        public PermanentTenant()
        {
            PermanentTenantParkingLots = new HashSet<PermanentTenantParkingLot>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DisplayName("ZIP Code")]
        public int ZipCode { get; set; }

        public ICollection<PermanentTenantParkingLot> PermanentTenantParkingLots { get; set; }
    }
}
