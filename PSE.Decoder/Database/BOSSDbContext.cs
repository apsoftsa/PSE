using Microsoft.EntityFrameworkCore;
using PSE.Model.BOSS;

namespace PSE.Decoder.Database
{

    internal class BOSSDbContext : DbContext
    {

        public DbSet<TmpAdordlat> AdOrdLat { get; set; }
        public DbSet<TmpAdaAnagr> AdaAnagr { get; set; }
        public DbSet<TmpAdaAuIde> AdaAuIde { get; set; }
        public DbSet<TmpTabelle> Tabelle { get; set; }

        public BOSSDbContext(DbContextOptions<BOSSDbContext> options) : base(options) { }

    }

}
