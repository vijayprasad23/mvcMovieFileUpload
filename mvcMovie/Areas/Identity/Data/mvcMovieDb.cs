using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvcMovie.Areas.Identity.Data;
using mvcMovie.Models;

namespace mvcMovie.Areas.Identity.Data;

public class mvcMovieDb : IdentityDbContext<mvcMovieUser>
{
    public mvcMovieDb(DbContextOptions<mvcMovieDb> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<mvcMovie.Models.movie> movie { get; set; } = default!;
}
