using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OpenMod.EntityFrameworkCore;
using OpenMod.EntityFrameworkCore.Configurator;

namespace WhitePlugin
{
    public class UserConnectionDbContext : OpenModDbContext<UserConnectionDbContext>
    {
        public UserConnectionDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public UserConnectionDbContext(IDbContextConfigurator configurator, IServiceProvider serviceProvider) : base(configurator, serviceProvider)
        {
        }

        public DbSet<UserConnection> UserConnections => Set<UserConnection>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserConnection>()
                .HasKey(x => x.ConnectionId);

            modelBuilder.Entity<UserConnection>()
                .Property(x => x.ConnectionId)
                .ValueGeneratedOnAdd();
        }
    }
}