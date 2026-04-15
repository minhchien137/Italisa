using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ItalisaTools.Models
{
    
    [Table("SVN_ProductMapping")]
    public class SVN_ProductMapping
    {

        public int product_id { get; set; }
        public string Operation { get; set; }

        public string Operation_Name { get; set; }

    }
}