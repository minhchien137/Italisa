// Bảng này chỉ để lưu dữ liệu của sản lượng và defect từ file của QC cung cấp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalisaTools.Models
{
    [Table("SVN_Italisa_Production_ByExcel")]
    public class SVN_Italisa_Production_ByExcel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? process { get; set; }
        public int? product_qty { get; set; }
        public int? product_id { get; set; }
        public string? color { get; set; }
        public string? vendor { get; set; }
        public string? type_value { get; set; }
        public string? defect_name { get; set; }
        public DateTime date_finished { get; set; } = DateTime.Now;

        public string? description { get; set; }
        public string? image_path { get; set; }
    }
}
