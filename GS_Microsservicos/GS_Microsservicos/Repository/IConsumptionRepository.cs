using GS_Microsservicos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GS_Microsservicos.Repositories
{
    public interface IConsumptionRepository
    {
        Task<IEnumerable<Consumptiondomain>> ListAll();
        Task<Consumptiondomain> GetById(string id); 
        Task Save(Consumptiondomain consumption);
        Task Update(Consumptiondomain consumption);
        Task Delete(string id); 
    }
}


