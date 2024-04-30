using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Interest
    {
        [Key]
        public int InterestId { get; set; }
        [Required]
        public string InterestName { get; set; }
        public string Description { get; set; }
        //public IEnumerable<PersonWithInterests>? people { get; set; }
    }
}
