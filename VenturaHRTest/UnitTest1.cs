using AspNetIdentityDbContext;
using DTO.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interface;
using Services.Opportunity;
using System;
using System.Threading.Tasks;
using VenturaHR.Controllers;
using Xunit;

namespace VenturaHRTest
{
    public class UnitTest1
    {
        //private AccountController _accountController;
        //private readonly Mock<HttpContext> _contextMock;

        //public UnitTest1()
        //{
        //    _contextMock = new Mock<HttpContext>();
        //}

        [Fact(DisplayName = "Create Opportuniry Success")]
        public async Task Create_Opportunity_Sucess()
        {
            var opportunity = new Mock<IOpportunityService>();
            var model = new DTO.Opportunity.OpportunityViewModel()
            {
                StatusId = 1,
                Office = "Analista Senior",
                Description = "2 anos de experiência",
                EmploymentId = 2,
                Salary = 3000.00,
                ExpirationDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CompanyId = 3,
                IsDeleted = false,
            };

            opportunity.Object.Create(model);

            opportunity.Verify(r => r.Create(model), Times.Once);
        }
    }
}
