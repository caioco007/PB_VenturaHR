using DTO.Account;
using DTO.Person;
using DTO.Shared;
using DTO.User;
using DTO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Person;
using Services.User;
using System;
using System.Threading.Tasks;
using VenturaHR.Helpers;

namespace VenturaHR.Controllers
{
    public class AccountController : Controller
    {
        readonly SignInManager<AspNetIdentityDbContext.User> signInManager;
        readonly UserManager<AspNetIdentityDbContext.User> userManager;
        readonly UserService userService;
        readonly PersonService personService;

        readonly ViewEngineHelper viewEngineHelper;

        public AccountController(SignInManager<AspNetIdentityDbContext.User> signInManager, UserManager<AspNetIdentityDbContext.User> userManager, ViewEngineHelper viewEngineHelper, UserService userService, PersonService personService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.viewEngineHelper = viewEngineHelper;
            this.personService = personService;
            this.userService = userService;
        }

        #region [SignIn]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SignIn(string email)
        {
            await signInManager.SignOutAsync();

            return await Task.Run(() => View(new SignInViewModel() { Email = email }));
        }
        [AllowAnonymous]
        [HttpPost]
        [ActionName("SignIn")]
        public async Task<IActionResult> _SignIn(SignInViewModel model, string returnUrl = null, int? personType = null)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewData["ErrorMessage"] = "Por favor, preencha o e-mail.";
                return await Task.Run(() => View(model));
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ViewData["ErrorMessage"] = "Por favor, preencha a senha.";
                return await Task.Run(() => View(model));
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "E-mail ou senha incorretos.";
                return await Task.Run(() => View(model));
            }

            if (!user.IsActive || user.IsDeleted)
            {
                ViewData["ErrorMessage"] = "E-mail ou senha incorretos.";
                return await Task.Run(() => View(model));
            }

            var userViewModel = userService.ToViewModel(user.CopyToEntity<ApplicationDbContext.Models.AspNetUsers>());

            var personViewModel = personService.GetDataById(userViewModel.PersonId.Value);

            if (personViewModel.PersonTypeId != personType)
            {
                if (userViewModel.RoleName == "Company" && personType==(int)DTO.Person.PersonType.Candidate)
                {
                    ViewData["ErrorMessage"] = "Esse usuário não possuí conta de Candidato";
                    return await Task.Run(() => View(model));
                }
                else if (userViewModel.RoleName == "Company" && personType == (int)DTO.Person.PersonType.Administrator)
                {
                    ViewData["ErrorMessage"] = "Esse usuário não possuí conta de Administrador";
                    return await Task.Run(() => View(model));
                }
                else if (userViewModel.RoleName == "Candidate" && personType == (int)DTO.Person.PersonType.Company)
                {
                    ViewData["ErrorMessage"] = "Esse usuário não possuí conta de Empresa";
                    return await Task.Run(() => View(model));
                }
                else if (userViewModel.RoleName == "Candidate" && personType == (int)DTO.Person.PersonType.Administrator)
                {
                    ViewData["ErrorMessage"] = "Esse usuário não possuí conta de Administrador";
                    return await Task.Run(() => View(model));
                }
                else if (userViewModel.RoleName == "Administrator" && personType == (int)DTO.Person.PersonType.Candidate)
                {
                    ViewData["ErrorMessage"] = "Esse usuário não possuí conta de Candidato";
                    return await Task.Run(() => View(model));
                }
                else if (userViewModel.RoleName == "Administrator" && personType == (int)DTO.Person.PersonType.Company)
                {
                    ViewData["ErrorMessage"] = "Esse usuário não possuí conta de Empresa";
                    return await Task.Run(() => View(model));
                }
            }

            var signInResult = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            await this.signInManager.SignInAsync(user, false);
            if (signInResult.Succeeded)
            {
                return !string.IsNullOrWhiteSpace(returnUrl) && returnUrl != "/" ? (IActionResult)(await Task.Run(() => Redirect(returnUrl))) : (IActionResult)(await Task.Run(() => RedirectToAction("Index", "Home")));
            }

            ViewData["ErrorMessage"] = "E-mail ou senha incorretos.";
            return await Task.Run(() => View(model));
        }
        #endregion

        #region [Register]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register(int? personType)
        {
            return await Task.Run(() => View(personType));
        }
        [AllowAnonymous]
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> _Register(UserViewModel model, string returnUrl = null, int? personType = null)
        {
            if (string.IsNullOrWhiteSpace(model.FirstName))
                return await Task.Run(() => Json(new ReturnResult(null, "Por favor, preencha o nome", true)));

            if (!model.Id.HasValue && (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6))
                return await Task.Run(() => Json(new ReturnResult(null, "Por favor, preencha uma senha de no mínimo 6 caracteres.", true)));

            if (!string.IsNullOrWhiteSpace(model.Password) && model.Password != model.PasswordConfirmation)
                return await Task.Run(() => Json(new ReturnResult(null, "Senha e confirmação não coincidem.", true)));

            if (!model.Id.HasValue && await this.userService.EmailExists(model.Email))
                return await Task.Run(() => Json(new ReturnResult(null, "Este e-mail já está sendo utilizado.", true)));

            if (model.PersonId == (int)DTO.Person.PersonType.Company) model.RoleName = DTO.Person.PersonType.Company.ToString();
            else if (model.PersonId == (int)DTO.Person.PersonType.Candidate) model.RoleName = DTO.Person.PersonType.Candidate.ToString();

            var personId = CreatePerson(model, personType.Value);

            model.PersonId = personId.Result;

            model.RoleName = await personService.GetRoleName(personId.Result);

            var userId = await this.userService.CreateOrUpdateAsync(model);

            return await Task.Run(() => RedirectToAction("SignIn"));
        }

        private async Task<int> CreatePerson(UserViewModel model, int personType)
        {
            var personViewModel = new PersonViewModel
            {
                PersonTypeId = personType,
                CompanyName = model.FullName,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                IsActive = true,
                CreatedDate = DateTime.Now,
            };

            return personService.Create(personViewModel);
        }


        #endregion

        #region [Logout]
        public async Task<IActionResult> SignOut()
        {
            await this.signInManager.SignOutAsync();
            return await Task.Run(() => RedirectToAction("SignIn"));
        }
        #endregion

        #region [PesswordRecovery]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordRecovery() => await Task.Run(() => View());
        [HttpPost]
        [ActionName("PasswordRecovery")]
        [AllowAnonymous]
        public async Task<IActionResult> _PasswordRecovery(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "E-mail não encontrado.";
                    return await Task.Run(() => RedirectToAction(nameof(PasswordRecovery), new { notfound = true }));
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

                var model = new PasswordRecoveryEmailViewModel(new DTO.User.UserViewModel { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email }, callback);
                var html = await viewEngineHelper.RenderPartialViewToString(ControllerContext, ViewData, TempData, "PasswordRecoveryEmail", model);

                return await Task.Run(() => RedirectToAction(nameof(PasswordRecovery), new { emailsent = true }));
            }
            catch (Exception exception)
            {
                return await Task.Run(() => RedirectToAction(nameof(PasswordRecovery), new { error = true }));
            }
        }
        #endregion

        #region [ResetPassword]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            return await Task.Run(() => View(new DTO.Account.ResetPasswordViewModel(token, email)));
        }
        [HttpPost]
        [ActionName("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> _ResetPassword(DTO.Account.ResetPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) RedirectToAction(nameof(ResetPassword));

            var resetPasswordResult = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!resetPasswordResult.Succeeded)
            {
                foreach (var error in resetPasswordResult.Errors)
                    ModelState.TryAddModelError(error.Code, error.Description);

                return await Task.Run(() => RedirectToAction(nameof(ResetPassword), new { token = model.Token, email = model.Email, error = true }));
            }

            return await Task.Run(() => RedirectToAction(nameof(SignIn), new { passwordchanged = true }));
        }
        #endregion

    }
}
