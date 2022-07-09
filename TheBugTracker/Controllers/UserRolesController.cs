using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBugTracker.Extensions;
using TheBugTracker.Models;
using TheBugTracker.Models.ViewModels;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly IBTRolesService _rolesService;
        private readonly IBTCompanyInfoService _companyInfoService;

        public UserRolesController(IBTRolesService rolesService, IBTCompanyInfoService companyInfoService)
        {
            _rolesService = rolesService;
            _companyInfoService = companyInfoService;
        }

        [HttpGet]
        public async Task <IActionResult> ManageUserRoles()
        {

            //Add an instnace of the ViewModel as a list
            List<ManageUserRolesViewModel> model = new();

            //Get CompanyId
            int companyId = User.Identity.GetCompanyId().Value;


            //Get All company users
            List<BTUser> users = await _companyInfoService.GetAllMembersAsync(companyId);

            //Loop over the users to populate the ViewModel
            // - instantiate ViewModel
            // - use _rolesService
            // - Create multi-selects
            foreach (BTUser user in users)
            {
                ManageUserRolesViewModel viewModel = new();
                viewModel.BTUser = user;
                IEnumerable<string> selected = await _rolesService.GetUserRolesAsync(user);
                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAsync(), "Name", "Name", selected);

                model.Add(viewModel);

            }

          
            // Return the model to the view
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {
            //Get the company Id
            int companyId = User.Identity.GetCompanyId().Value;
            //Instantiate the BTUser

            BTUser user = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == member.BTUser.Id);

            //Get Roles for the User
            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(user);

            //Grab the selected role
            string userRole = member.SelectedRoles.FirstOrDefault();

            if (!string.IsNullOrEmpty(userRole))
            {
                //Remove user from their roles
                if (await _rolesService.RemoveUserFromRolesAsync(user, roles))
                {
                    //Add user to the new role
                    await _rolesService.AddUserToRoleAsync(user, userRole);
                }
            }

            //Manage navigation
            return RedirectToAction(nameof(ManageUserRoles));
            
        }
    }
}
