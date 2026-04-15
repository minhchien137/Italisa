using Microsoft.EntityFrameworkCore;


namespace ItalisaTools.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SVN_Italisa_Production> SVN_Italisa_Production { get; set; }
        public DbSet<SVN_Italisa_vendor> SVN_Italisa_vendor { get; set; }
        public DbSet<SVN_Italisa_Code> SVN_Italisa_Code { get; set; }

        public DbSet<SVN_Italisa_Process> SVN_Italisa_Process { get; set; }

        public DbSet<SVN_ProductMapping> SVN_ProductMapping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SVN_ProductMapping>().HasNoKey();
        }

    } 

    
}
