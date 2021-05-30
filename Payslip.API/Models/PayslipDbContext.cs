using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Models
{
    public class PayslipDbContext : DbContext
    {
        public PayslipDbContext(DbContextOptions<PayslipDbContext> options) : base(options)
        {
        }

        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<TaxRateLevel> TaxRateLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
