using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalisaTools.Models
{
    [Table("SVN_Italisa_Production")]
    public class SVN_Italisa_Production
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? process { get; set; }
        public int? product_qty { get; set; }
        public int? product_id { get; set; }
        public string? vendor { get; set; }
        public string? type_value { get; set; }
        public DateTime date_finished { get; set; } = DateTime.Now;

        public string? description { get; set; }
        public string? image_path { get; set; }
    }
}
