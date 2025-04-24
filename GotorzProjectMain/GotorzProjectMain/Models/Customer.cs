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

        [Required]
        public string CustomUserName { get; set; }

        public ApplicationUser User { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; } = new byte[8];
	}

}
