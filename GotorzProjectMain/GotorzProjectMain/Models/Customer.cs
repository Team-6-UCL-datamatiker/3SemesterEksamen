using GotorzProjectMain.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models
{
    public class Customer
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        public string Username { get; set; }

        public ApplicationUser User { get; set; }
    }

}
