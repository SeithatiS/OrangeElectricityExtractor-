namespace OrangeTransactionsExtractor
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=Model2")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        //public virtual DbSet<RequestType> RequestTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.Client)
                .HasForeignKey(e => e.Client_Client_Id);
        }
    }
}
