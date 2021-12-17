using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("LawsuitTypes", Schema = "dbo")]
    public class LawsuitType
    {
        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string name { get; set; }

        [Column("lawsuitTypeDictionaryID", TypeName = "int")]
        public int? lawsuitTypeDictionaryID { get; set; }

        [ForeignKey("lawsuitTypeDictionaryID")]
        public virtual LawsuitTypeDictionary typeDictionary { get; set; }
    }
}
