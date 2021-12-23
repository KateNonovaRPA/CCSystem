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

        public Guid identityID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? deletedAt { get; set; }
    }
}