using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Boxer.UI.Models;

namespace Boxer.UI.Data
{
    public class BoxerUIContext : DbContext
    {
        public BoxerUIContext (DbContextOptions<BoxerUIContext> options)
            : base(options)
        {
        }

        public DbSet<Boxer.UI.Models.Orders> Orders { get; set; } = default!;
    }
}
