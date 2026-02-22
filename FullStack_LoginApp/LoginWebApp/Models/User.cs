using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginWebApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = "";

        [Column(TypeName = "nvarchar(100)")]
        public string Surname { get; set; } = "";

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = "";

        [Column(TypeName = "nvarchar(100)")]
        public string Password { get; set; } = "";

        [Column(TypeName = "nvarchar(9)")]
        public string Phone { get; set; } = "";
    }
}
