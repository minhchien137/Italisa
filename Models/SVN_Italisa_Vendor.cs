using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalisaTools.Models
{

    [Table("SVN_Italisa_vendor")]
    public class SVN_Italisa_vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? vendor_code { get; set; }

        public string? vendor { get; set; }

       
    }
}