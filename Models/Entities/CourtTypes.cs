using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Models.Entities
{
    public class CourtType
    {
        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int courtId { get; set; }
        public Court Court { get; set; }

        public int typeID { get; set; }
        public Type Type { get; set; }
    }
}
