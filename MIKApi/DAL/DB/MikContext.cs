using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MIKApi.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIKApi.DAL.DB
{
    public class MikContext: ApiAuthorizationDbContext<ApplicationUser>
    {
        public MikContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        //public MikContext(DbContextOptions<MikContext> options) : base(options)
        //{
        //}

        public DbSet<Nauha> Nauhas { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<LocationGroup> LocationGroups { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}
