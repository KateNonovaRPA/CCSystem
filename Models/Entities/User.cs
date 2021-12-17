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

        public string processorID { get; set; }
        public string administrationName { get; set; }
        public string name { get; set; }
        public string email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? deletedAt { get; set; }
    }
}