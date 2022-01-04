using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Users", Schema = "dbo")]
    public class User
    {
        [Key]
        [Column("UUID")]
        public Guid UUID { get; set; }
        [Required]
        public Guid ClientID { get; set; }

        public string AccessToken { get; set; }

        public string FullName { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? deletedAt { get; set; }
    }
}