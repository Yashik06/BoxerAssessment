using Boxer.DL.DTOProperties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.DL
{
    public class BoxerDBContext : DbContext
    {
        public BoxerDBContext(DbContextOptions<BoxerDBContext> options)
            : base(options)
        {
        }

        public DbSet<Orders> Orders { get; set; } = default!;

    }
}
