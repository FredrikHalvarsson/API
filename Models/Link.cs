using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Link
    {
        [Key]
        public int LinkId { get; set; }
        public string URL { get; set; }
        [ForeignKey("PersonWithInterest")]
        public int PersonWithInterestsId { get; set; }
    }
}
