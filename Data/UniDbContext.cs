using Microsoft.EntityFrameworkCore;

public class UniDbContext : DbContext
{
    public UniDbContext(DbContextOptions<UniDbContext> options) : base(options)
    {
        
    }   

    public DbSet<Docente> Docentes {get; set;}
}