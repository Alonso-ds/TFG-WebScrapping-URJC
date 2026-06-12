using Microsoft.EntityFrameworkCore;

public class UniDbContext : DbContext
{
    public UniDbContext(DbContextOptions<UniDbContext> options) : base(options)
    {
        
    }   

    public DbSet<Docente> Docentes {get; set;}
    public DbSet<Proyecto> Proyectos {get; set;}
    //public DbSet<Publicacion> Publicaciones {get; set;}
    //public DbSet<Asignatura> Asignaturas {get; set;}
    //public DbSet<TFE> TFEs {get; set;}

}