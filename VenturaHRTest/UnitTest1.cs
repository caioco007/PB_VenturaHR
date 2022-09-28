using AspNetIdentityDbContext;
using DTO.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using VenturaHR.Controllers;
using Xunit;

namespace VenturaHRTest
{
    public class UnitTest1
    {
        private AccountController _accountController;
        private readonly Mock<HttpContext> _contextMock;

        public UnitTest1()
        {
            _contextMock = new Mock<HttpContext>();
        }

        [Fact(DisplayName = "Validate Login Success")]
        public async Task AccountController_SignIn_Sucess()
        {
            var model = new SignInViewModel()
            {
                Email = "caio@gmail.com",
                Password = "123456",
                RememberMe = false
            };
            //model.Email = "caiocandidate@gmail.com";
            //model.Password = "123456";
            //model.RememberMe = false;
            

            _accountController.ControllerContext.HttpContext = _contextMock.Object;
            var signInResult = await _accountController._SignIn(model,null,2);
            var viewResult = Assert.IsType<OkObjectResult>(signInResult);

            Assert.IsAssignableFrom<SignInViewModel>(viewResult.Value);
        }
    }
}
