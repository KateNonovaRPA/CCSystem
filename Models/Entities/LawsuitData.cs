using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("LawsuitData", Schema = "dbo")]
    public class LawsuitData
    {
        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("courtAttributeID", TypeName = "int")]
        public int courtAttributeID { get; set; }

        [ForeignKey("courtAttributeID")]
        public CourtAttribute CourtAttribute { get; set; }

        public string data { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }

        [Column("lawsuitID", TypeName = "int")]
        public int? lawsuitID { get; set; }

        [ForeignKey("lawsuitID")]
        public virtual Lawsuit Lawsuit { get; set; }

        public int changeNumber { get; set; }
    }
}