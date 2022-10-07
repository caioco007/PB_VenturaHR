using DTO.Person;
using DTO.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IPersonService : IRepositoryService<ApplicationDbContext.Models.Person, PersonViewModel, int>
    {
        ReturnResult ValidateNewPerson(PersonViewModel model);
        Task<ReturnResult> ValidateNewPersonAsync(PersonViewModel model);
        Task<ReturnResult> ValidateDelete(int personId);
        int GetPersonTypeIdByPersonId(int personId);
        Task<ApplicationDbContext.Models.Person> GetDataByCNPCPFAsync(string cnpjCpf);
        ApplicationDbContext.Models.Person GetDataByCNPCPF(string cnpjCpf);
        Task<string> GetRoleName(int personId);
        Task BlockPerson(int personId, string motivo);
        Task ActivePerson(int personId);
    }
}
