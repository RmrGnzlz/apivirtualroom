using System.Collections.Generic;
using Domain.Contracts;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEstudianteRepository : IGenericRepository<Estudiante>
    {
        IEnumerable<Estudiante> Search(string search);
    }
}