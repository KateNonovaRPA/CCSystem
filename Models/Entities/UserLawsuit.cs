using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("UserLawsuits", Schema = "dbo")]
    public class UserLawsuit
    {
        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public bool active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; }

        [Column("userID")]
        public Guid userID { get; set; }

        [ForeignKey("userID")]
        public User User { get; set; }

        [Column("lawsuitID", TypeName = "int")]
        public int lawsuitID { get; set; }

        [ForeignKey("lawsuitID")]
        public Lawsuit Lawsuit { get; set; }
    }
}