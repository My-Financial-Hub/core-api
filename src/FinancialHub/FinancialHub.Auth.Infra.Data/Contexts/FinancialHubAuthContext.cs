using FinancialHub.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialHub.Auth.Infra.Data.Contexts
{
    public class FinancialHubAuthContext : DbContext
    {
        public FinancialHubAuthContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
