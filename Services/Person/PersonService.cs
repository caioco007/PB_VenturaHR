using DTO.Person;
using DTO.Shared;
using DTO.User;
using DTO.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Person
{
    public class PersonService : Shared.RepositoryService<ApplicationDbContext.Models.Person, PersonViewModel, int>
    {
        public PersonService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "PersonId")
        {
        }

        public ReturnResult ValidateNewPerson(PersonViewModel model)
        {
            if (this.dbSet.Any(x => (!string.IsNullOrWhiteSpace(model.Cpf) && x.Cpf == model.CpfNumbers && !x.IsDeleted && x.PersonId != model.PersonId)))
                return new ReturnResult(null, "O CPF informado já esta em uso", true);

            if (this.dbSet.Any(x => (!string.IsNullOrWhiteSpace(model.Cnpj) && x.Cnpj == model.CnpjNumbers && !x.IsDeleted && x.PersonId != model.PersonId)))
                return new ReturnResult(null, "O CNPJ informado já esta em uso", true);

            return new ReturnResult(null, null, false);
        }

        public async Task<ReturnResult> ValidateNewPersonAsync(PersonViewModel model)
        {
            if (await this.dbSet.AnyAsync(x => (!string.IsNullOrWhiteSpace(model.Cpf) && x.Cpf == model.CpfNumbers && !x.IsDeleted && x.PersonId != model.PersonId)))
                return new ReturnResult(null, "O CPF informado já esta em uso", true);

            if (await this.dbSet.AnyAsync(x => (!string.IsNullOrWhiteSpace(model.Cnpj) && x.Cnpj == model.CnpjNumbers && !x.IsDeleted && x.PersonId != model.PersonId)))
                return new ReturnResult(null, "O CNPJ informado já esta em uso", true);

            return new ReturnResult(null, null, false);
        }

        public async Task<ReturnResult> ValidateDelete(int personId) => await Task.Run(async () =>
        {
            return new ReturnResult(null, null, false);
        });

        public int GetPersonTypeIdByPersonId(int personId) => GetDataById(personId).PersonTypeId;

        public async Task<ApplicationDbContext.Models.Person> GetDataByCNPCPFAsync(string cnpjCpf)
        {
            var cnpjCpfNumbersOnly = cnpjCpf.NumbersOnly();

            if (cnpjCpfNumbersOnly.Length == 11) return await this.dbSet.SingleOrDefaultAsync(x => x.Cpf == cnpjCpfNumbersOnly);

            return await this.dbSet.SingleOrDefaultAsync(x => x.Cnpj == cnpjCpfNumbersOnly && !x.IsDeleted);
        }

        public ApplicationDbContext.Models.Person GetDataByCNPCPF(string cnpjCpf)
        {
            var cnpjCpfNumbersOnly = cnpjCpf.NumbersOnly();

            if (cnpjCpfNumbersOnly.Length == 11) return this.dbSet.SingleOrDefault(x => x.Cpf == cnpjCpfNumbersOnly);

            return this.dbSet.SingleOrDefault(x => x.Cnpj == cnpjCpfNumbersOnly && !x.IsDeleted);
        }

        public override async Task<int> CreateAsync(PersonViewModel model)
        {
            var person = new ApplicationDbContext.Models.Person
            {
                PersonTypeId = model.PersonTypeId,
                CompanyName = model.CompanyName,
                TradeName = model.TradeName,
                Cnpj = model.Cnpj,
                FullName = model.FullName,
                Cpf = model.Cpf,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                MobileNumber = model.MobileNumber,
                CreatedDate = model.CreatedDate,
                IsActive = model.IsActive
            };

            this.dbSet.Add(person);
            this.context.SaveChanges();

            return person.PersonId;
        }

        public override async Task UpdateAsync(PersonViewModel model)
        {
            var person = GetDataById(model.PersonId.Value);

            person.CompanyName = model.CompanyName;
            person.TradeName = model.TradeName;
            person.Cnpj = model.Cnpj;
            person.FullName = model.FullName;
            person.Cpf = model.Cpf;
            person.Email = model.Email;
            person.PhoneNumber = model.PhoneNumber;
            person.MobileNumber = model.MobileNumber;

            this.dbSet.Update(person);
            this.context.SaveChanges();
        }

        public async Task<string> GetRoleName(int personId)
        {
            var person = GetDataById(personId);

            string roleName = "";
            switch (person.PersonTypeId)
            {
                case 1:
                    roleName = "Administrator";
                    break;
                case 2:
                    roleName = "Company";
                    break;
                case 3:
                    roleName = "Candidate";
                    break;
            }
            return roleName;
        }
        
        public async Task BlockPerson(int personId, string motivo)
        {
            var person = GetDataById(personId);

            person.IsActive = false;
            person.BlockDate = DateTime.Now;
            person.BlockReason = motivo;

            this.dbSet.Update(person);
            this.context.SaveChanges();
        }

        public async Task ActivePerson(int personId)
        {
            var person = GetDataById(personId);

            person.IsActive = true;
            person.BlockDate = null;
            person.BlockReason = "";

            this.dbSet.Update(person);
            this.context.SaveChanges();
        }
    }
}
