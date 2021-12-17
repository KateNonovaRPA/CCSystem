using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("Lawsuits", Schema = "dbo")]
    public class Lawsuit
    {
        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string lawsuitNumber { get; set; }

        [Column("typeID", TypeName = "int")]
        public int typeID { get; set; }

        [ForeignKey("typeID")]
        public LawsuitType Type { get; set; }

        [Column("courtID", TypeName = "int")]
        public int courtID { get; set; }

        [ForeignKey("courtID")]
        public Court Court { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; }

        public virtual IList<LawsuitData> LawsuitDatas { get; set; }
    }
}
