using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalisaTools.Models
{
    [Table("SVN_Italisa_Code")]
    public class SVN_Italisa_Code
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int? Italisa_no { get; set; }
        
        public string? Item_code { get; set; }
    }   
}