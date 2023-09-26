using Microsoft.EntityFrameworkCore;
using PSE.Decoder.Database.Model;

namespace PSE.Decoder.Database
{

    internal class BOSSDbContext : DbContext
    {

        public DbSet<TmpAdaAuIde> AdaAuIde { get; set; }
        public DbSet<TmpTabelle> Tabelle { get; set; }

        public BOSSDbContext(DbContextOptions<BOSSDbContext> options) : base(options) { }

    }

}
