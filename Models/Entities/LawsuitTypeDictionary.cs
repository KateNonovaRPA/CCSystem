using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("LawsuitTypeDictionary", Schema = "dbo")]
    public class LawsuitTypeDictionary
    {

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string name { get; set; }
        public virtual IList<LawsuitType> LawsuitTypes { get; set; }
    }
}
