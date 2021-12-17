using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    public class Court
    {
        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string fullName { get; set; }
        public string name { get; set; }

        [Column("cityId", TypeName = "int")]
        public int? cityId { get; set; }

        [ForeignKey("cityId")]
        public City City { get; set; }
       
        [DataType(DataType.DateTime)]
        public DateTime createdAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; }

        public IList<CourtType> CourtTypes { get; set; }
        public IList<Lawsuit> Lawsuits { get; set; }
        public IList<CourtAttribute> CourtAttributes { get; set; }

    }
}
