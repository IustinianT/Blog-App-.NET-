using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog_App.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog_App.Models.Note> Note { get; set; } = default!;
    }
}
