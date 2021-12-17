using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("CourtAttributes", Schema = "dbo")]
    public class CourtAttribute
    {
        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }        
        public string attributeName { get; set; }

        [Column("courtID", TypeName = "int")]
        public int parentID { get; set; }
        public int courtID { get; set; }

        [ForeignKey("courtID")]
        public Court Court { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }
    }
}
