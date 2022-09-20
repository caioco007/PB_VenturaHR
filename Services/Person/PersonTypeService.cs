using ApplicationDbContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Person
{
    public class PersonTypeService : Shared.RepositoryService<PersonType, PersonType, int>
    {
        public PersonTypeService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "PersonTypeId")
        {
        }

        public async Task<PersonType> GetDataByExternalCode(string externalCode) => await this.dbSet.SingleAsync(x => x.ExternalCode == externalCode);
    }
}
