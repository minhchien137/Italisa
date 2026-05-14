using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItalisaTools.Models
{
    [Table("SVN_Italisa_DefectInfor")]
    public class SVN_Italisa_DefectInfor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? defect_name_en { get; set; }
        public string? defect_name_vn { get; set; }
        public string? defect_name_cn { get; set; }
    }
}
