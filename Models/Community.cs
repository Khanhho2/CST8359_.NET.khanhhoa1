using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A1.Models
{
    public class Community
    {
        [Required]
        [Display(Name = "Registration Number")]
        public string ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }
        public ICollection<CommunityMembership> Membership { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
