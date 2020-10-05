using Domain.Contracts;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _dbContext;
        public UnitOfWork(IDbContext context)
        {
            _dbContext = context;
        }

        #region PrivateRepositories
        private IActividadRepository  _actividadRepository;
        private IAsignaturaRepository _asignaturaRepository;
        private IClaseRepository _claseRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IDirectivoRepository _directivoRepository;
        private IDocenteRepository _docenteRepository;
        private IEstudianteRepository _estudianteRepository;
        private IGradoRepository _gradoRepository;
        private IGrupoRepository _grupoRepository;
        private IGrupoAsignaturaRepository _grupoAsignaturaRepository;
        private IGrupoAsignaturaClaseRepository _grupoAsignaturaClaseRepository;
        private IHorarioRepository _horarioRepository;
        private IInstitucionRepository _institucionRepository;
        private IMultimediaRepository _multimediaRepository;
        private IMunicipioRepository _municipioRepository;
        private IPersonaRepository _personaRepository;
        private IRolRepository _rolRepository;
        private ISedeRepository _sedeRepository;
        private IUsuarioRepository _usuarioRepository;
        #endregion
        #region PublicRepositories
        public IActividadRepository ActividadRepository
        {
            get { return _actividadRepository ?? (_actividadRepository = new ActividadRepository(_dbContext)); }
        }
        public IAsignaturaRepository AsignaturaRepository
        {
            get { return _asignaturaRepository ?? (_asignaturaRepository = new AsignaturaRepository(_dbContext)); }
        }
        public IClaseRepository ClaseRepository
        {
            get { return _claseRepository ?? (_claseRepository = new ClaseRepository(_dbContext)); }
        }
        public IDepartamentoRepository DepartamentoRepository
        {
            get { return _departamentoRepository ?? (_departamentoRepository = new DepartamentoRepository(_dbContext)); }
        }
        public IDirectivoRepository DirectivoRepository
        {
            get { return _directivoRepository ?? (_directivoRepository = new DirectivoRepository(_dbContext)); }
        }
        public IDocenteRepository DocenteRepository
        {
            get { return _docenteRepository ?? (_docenteRepository = new DocenteRepository(_dbContext)); }
        }
        public IEstudianteRepository EstudianteRepository
        {
            get { return _estudianteRepository ?? (_estudianteRepository = new EstudianteRepository(_dbContext)); }
        }
        public IGradoRepository GradoRepository
        {
            get { return _gradoRepository ?? (_gradoRepository = new GradoRepository(_dbContext)); }
        }
        public IGrupoRepository GrupoRepository
        {
            get { return _grupoRepository ?? (_grupoRepository = new GrupoRepository(_dbContext)); }
        }
        public IGrupoAsignaturaRepository GrupoAsignaturaRepository
        {
            get { return _grupoAsignaturaRepository ?? (_grupoAsignaturaRepository = new GrupoAsignaturaRepository(_dbContext)); }
        }
        public IGrupoAsignaturaClaseRepository GrupoAsignaturaClaseRepository
        {
            get { return _grupoAsignaturaClaseRepository ?? (_grupoAsignaturaClaseRepository = new GrupoAsignaturaClaseRepository(_dbContext)); }
        }
        public IHorarioRepository HorarioRepository
        {
            get { return _horarioRepository ?? (_horarioRepository = new HorarioRepository(_dbContext)); }
        }
        public IInstitucionRepository InstitucionRepository
        {
            get { return _institucionRepository ?? (_institucionRepository = new InstitucionRepository(_dbContext)); }
        }
        public IMultimediaRepository MultimediaRepository
        {
            get { return _multimediaRepository ?? (_multimediaRepository = new MultimediaRepository(_dbContext)); }
        }
        public IMunicipioRepository MunicipioRepository
        {
            get { return _municipioRepository ?? (_municipioRepository = new MunicipioRepository(_dbContext)); }
        }
        public IPersonaRepository PersonaRepository
        {
            get { return _personaRepository ?? (_personaRepository = new PersonaRepository(_dbContext)); }
        }
        public IRolRepository RolRepository
        {
            get { return _rolRepository ?? (_rolRepository = new RolRepository(_dbContext)); }
        }
        public ISedeRepository SedeRepository
        {
            get { return _sedeRepository ?? (_sedeRepository = new SedeRepository(_dbContext)); }
        }
        public IUsuarioRepository UsuarioRepository
        {
            get { return _usuarioRepository ?? (_usuarioRepository = new UsuarioRepository(_dbContext)); }
        }
        #endregion

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                ((DbContext)_dbContext).Dispose();
                _dbContext = null;
            }
        }
    }
}
