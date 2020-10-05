using Domain.Entities;
using Domain.Values;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SolumaticaContext : DbContextBase
    {
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<ActividadMultimedia> ActividadMultimedias { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Directivo> Directivos { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Grado> Grados { get; set; }
        public DbSet<GradoDocente> GradoDocentes { get; set; }
        public DbSet<GradoAsignatura> GradoAsignaturas { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<GrupoEstudiante> GrupoEstudiantes { get; set; }
        public DbSet<GrupoAsignatura> GrupoAsignaturas { get; set; }
        public DbSet<GrupoAsignaturaClase> GrupoAsignaturaClases { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Institucion> Instituciones { get; set; }
        public DbSet<Multimedia> Multimedias { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<SedeDocente> SedeDocentes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public SolumaticaContext(DbContextOptions options) : base(options)
        {
            // Cualquier cambio
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>()
            .HasOne(p => p.Usuario)
            .WithOne(u => u.Persona)
            .HasForeignKey<Usuario>(u => u.PersonaId);
            modelBuilder.Entity<GradoAsignatura>().HasKey(x => new { x.AsignaturaId, x.GradoId });
            modelBuilder.Entity<GrupoEstudiante>().HasKey(x => new { x.GrupoId, x.EstudianteId });
            modelBuilder.Entity<GradoDocente>().HasKey(x => new {x.DocenteId, x.GradoId});
            modelBuilder.Entity<ActividadMultimedia>().HasKey(x => new { x.ActividadId, x.MultimediaId });
            modelBuilder.Entity<SedeDocente>().HasKey(x => new { x.SedeId, x.DocenteId });
            modelBuilder.Entity<GrupoAsignaturaClase>().HasKey(x => new { x.ClaseId, x.GrupoAsignaturaId });
        }
    }
}
