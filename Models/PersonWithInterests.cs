using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class PersonWithInterests
    {
        public int Id { get; set; }
        public IEnumerable<Link> Links { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        [ForeignKey("Interest")]
        public int InterestId { get; set; }
        public Interest? Interest { get; set; }
    }
}
