using System;
using Domain.Entities;
using Domain.Repositories;

namespace Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repositories
        IActividadRepository ActividadRepository { get; }
        IAsignaturaRepository AsignaturaRepository { get; }
        IClaseRepository ClaseRepository { get; }
        IDepartamentoRepository DepartamentoRepository { get; }
        IDirectivoRepository DirectivoRepository { get; }
        IDocenteRepository DocenteRepository { get; }
        IEstudianteRepository EstudianteRepository { get; }
        IGradoRepository GradoRepository { get; }
        IGrupoRepository GrupoRepository { get; }
        IGrupoAsignaturaRepository GrupoAsignaturaRepository { get; }
        IGrupoAsignaturaClaseRepository GrupoAsignaturaClaseRepository { get; }
        IHorarioRepository HorarioRepository { get; }
        IInstitucionRepository InstitucionRepository { get; }
        IMultimediaRepository MultimediaRepository { get; }
        IMunicipioRepository MunicipioRepository { get; }
        IPersonaRepository PersonaRepository { get; }
        IRolRepository RolRepository { get; }
        ISedeRepository SedeRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        #endregion
        int Commit();
    }
}
