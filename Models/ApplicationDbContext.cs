using Microsoft.EntityFrameworkCore;


namespace ItalisaTools.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Dữ liệu lấy tự nhập từ hệ thống
        public DbSet<SVN_Italisa_Production> SVN_Italisa_Production { get; set; }

        // Dữ liệu lấy từ QC
        public DbSet<SVN_Italisa_Production_ByExcel> SVN_Italisa_Production_ByExcel { get; set; }

        public DbSet<SVN_Italisa_vendor> SVN_Italisa_vendor { get; set; }
        public DbSet<SVN_Italisa_Code> SVN_Italisa_Code { get; set; }

        public DbSet<SVN_Italisa_Process> SVN_Italisa_Process { get; set; }

        public DbSet<SVN_Italisa_Color> sVN_Italisa_Color { get; set; }

        public DbSet<SVN_Italisa_DefectInfor> SVN_Italisa_DefectInfor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    } 

    
}
