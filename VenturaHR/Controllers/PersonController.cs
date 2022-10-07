using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.Controllers
{
    public class PersonController : Controller
    {
        readonly IPersonService personService;
        readonly PersonTypeService personTypeService;
        private readonly UserManager<AspNetIdentityDbContext.User> userManager;

        public PersonController(IPersonService personService, PersonTypeService personTypeService, UserManager<AspNetIdentityDbContext.User> userManager)
        {
            this.personService = personService;
            this.personTypeService = personTypeService;
            this.userManager = userManager;
        }

        [HttpPost]
        [ActionName("Manage")]
        public async Task<IActionResult> _Manage(DTO.Person.PersonViewModel model)
        {
            try
            {
                model.PersonId = await this.personService.CreateOrUpdateAsync(model);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return await Task.Run(() => RedirectToAction("Index", "User"));
        }

        public async Task<IActionResult> Manage(int? id)
        {
            var model = personService.GetViewModelById(id.Value);

            return await Task.Run(() => View("Manage", model));
        }
    }
}
