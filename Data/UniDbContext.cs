using Microsoft.EntityFrameworkCore;

public class UniDbContext : DbContext
{
    public UniDbContext(DbContextOptions<UniDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=universidad.db;Cache=Shared;");
    }

    public DbSet<Docente> Docentes { get; set; }
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Publicacion> Publicaciones { get; set; }
    public DbSet<Asignatura> Asignaturas {get; set;}
    public DbSet<Tfe> Tfes {get; set;}

}