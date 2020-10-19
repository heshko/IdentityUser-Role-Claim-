using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityWithAhmadRabie.Areas.Admin.Models;
using IdentityWithAhmadRabie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityWithAhmadRabie.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Administration")]
    //[Authorize(Policy ="Create Role")]
   
    public class AdministrationController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
    
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // list Roles
        [HttpGet]

        public async Task<IActionResult> ListRoles()
        {

            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // Create Role
        [HttpGet]

        public IActionResult CreateRole()
        {
            return View();
        }

        // Create Role
        [HttpPost]

        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            return View(model);
        }

        // Edit Role
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMassage = $"I cant find the role with Id = {id}";
                return View("NotFound", "Administeration");
            }
            RoleViewModelUpdate rolevm = new RoleViewModelUpdate
            {
                RoleName = role.Name,
                Id = role.Id
            };
           
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                if (await _userManager.IsInRoleAsync(user,role.Name))
                {
                    rolevm.Users.Add(user.UserName);
                }
            }
            return View(rolevm);
        }

        // Edit Role
        [HttpPost]
        public async Task<IActionResult> Edit(string id , RoleViewModelUpdate model)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMassage = $"I cant find the role with Id = {id}";
                return View("NotFound", "Administeration");
            }

            if (role != null && model.RoleName != null)
            {
                if (role.Name != model.RoleName)
                {
                    role.Name = model.RoleName;
                   IdentityResult result =  await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles", "Administration");
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, err.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }

            }

            else
            {
                return View(model);
            }
        }

        // Delete Role
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMassage = $"I cant find the role with Id = {id}";
                return View("NotFound", "Administeration");
            }
            RoleViewModelUpdate rolevm = new RoleViewModelUpdate
            {
                RoleName = role.Name
            };
            return View(rolevm);
        }

        // Delete Role
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {

                var users = await _userManager.GetUsersInRoleAsync(role.Name);

                if (users.Count == 0)
                {

                        IdentityResult result = await _roleManager.DeleteAsync(role);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("ListRoles", "Administration");
                        }
                        else
                        {
                            foreach (var err in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, err.Description);
                            }
                            RoleViewModel model = new RoleViewModel
                            {
                                RoleName = role.Name
                            };
                            return View(model);
                        }
                }
                else
                {
                        ViewBag.ErrorMassage = $"I cant Remove  the role with Id = {id} becouse this role has Users";
                        return View("NotFound", "Administeration");
                }

            }
            else
            {
                    ViewBag.ErrorMassage = $"I cant find the role with Id = {id}";
                    return View("NotFound", "Administeration");
            }
        }

        // Edit User In Role
        [HttpGet]
        public async Task<IActionResult> EdiUserInRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            ViewBag.RoleName = role.Name;
            List<UserRoleViewModel> usersRole = new List<UserRoleViewModel>();
            if (role == null)
            {
                ViewBag.ErrorMassage = $"I cant find the role with Id = {id}";
                return View("NotFound", "Administeration");
            }
            else
            {
                var users = await _userManager.Users.ToListAsync();
                if (users.Any())
                {
                    foreach (var user in users)
                    {
                        UserRoleViewModel userRole = new UserRoleViewModel
                        {
                            UserId = user.Id,
                            UserName = user.UserName,

                        };
                        if (await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            userRole.isSelected = true;
                           
                        }
                        else
                        {
                            userRole.isSelected = false;
                          
                        }
                        usersRole.Add(userRole);

                    }
                }
              
               
              
            }
            return View(usersRole);

        }

        // Edit User In Role
        [HttpPost]
        public async Task<IActionResult> EdiUserInRole(List<UserRoleViewModel> userRoleViewModels,string id)
        {
            if(!userRoleViewModels.Any())
            {
                ViewBag.ErrorMassage = $"I cant find user Role";
                return View("NotFound", "Administeration"); ;
            }
            else
            {
                foreach (var userInRolo in userRoleViewModels)
                {
                    var user = await _userManager.FindByIdAsync(userInRolo.UserId);
                   var role2 = await _roleManager.FindByIdAsync(id);
                   
                    if ((userInRolo.isSelected) && !(await _userManager.IsInRoleAsync(user, role2.Name)))
                    {
                        var resuelt = await _userManager.AddToRoleAsync(user, role2.Name);
                        if (resuelt.Succeeded)
                        {

                        }
                        else
                        {
                            foreach (var error in resuelt.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            ViewBag.ErrorMassage = $"somtheing fel in server side";
                            return View("NotFound", "Administeration"); ;
                        }
                    }
                    else if (!(userInRolo.isSelected) && await (_userManager.IsInRoleAsync(user, role2.Name)))
                    {
                        var resuelt = await _userManager.RemoveFromRoleAsync(user, role2.Name);
                        if (resuelt.Succeeded)
                        {

                        }
                        else
                        {
                            foreach (var error in resuelt.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            ViewBag.ErrorMassage = $"somtheing fel in server side";
                            return View("NotFound", "Administeration"); ;
                        }
              
                    }
                    
                }
            }
            string roleName2 = ViewBag.RoleName;
            var role = await _roleManager.FindByIdAsync(id);
            return RedirectToAction("Edit", "Administration",new { id = role.Id });
        }

        // All Users
        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();

            return View(users);
        }

        // Edit User
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMassage = $"I cant find this user with id {id}";
                return RedirectToAction("NotFound", "Administration");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            var mdoel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Street = user.Street,
                City=user.City,
                Zipcode = user.Zipcode,
                Roles = roles.ToList(),
                Claims = claims.Select(x => x.Value).ToList()

            };

            return View(mdoel);
        }

        // Edit User
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMassage = $"I cant find this user with id {model.Id}";
                return RedirectToAction("NotFound", "Administration");
            }
            ///

            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(model.Email);

                if (userByEmail == null)
                {
                    user.Email = model.Email;
                }
               
              
              
                user.UserName = model.UserName.Trim();
                user.Street = model.Street;
                user.Zipcode = model.Zipcode;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Administration");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            
            }
            return View(model);
        }

        // Delete User
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"I cant find this user with id {id}";
                return RedirectToAction("NotFound", "Administration");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            var mdoel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Street = user.Street,
                City = user.City,
                Zipcode = user.Zipcode,
                Roles = roles.ToList(),
                Claims = claims.Select(x => x.Value).ToList()

            };

            return View(mdoel);

        }

        // Delete User
        [HttpPost]
        public async Task<IActionResult> DeleteUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"I cant find this user with id {model.Id}";
                return RedirectToAction("NotFound", "Administration");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers", "Administration");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                 
                }
            }

            return View(model);

        }

        // Manage user Claim

          [HttpGet] 
          public async Task<IActionResult> ManageUserClaim(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"I cant find this user with id {id}";
                return RedirectToAction("NotFound", "Administration");
            }
            var claims = await _userManager.GetClaimsAsync(user);
            UserClaimViewModel userClaim = new UserClaimViewModel
            {
                Id = id
            };
            if (!claims.Any())
            {
                foreach (var claim in ListClaims.claims)
                {

                    userClaim.Claims.Add(new UserClaim { ClaimType = claim.Type, });
                }
            }
            else {
                bool isSelected;
                foreach (var claim in ListClaims.claims)
                {
                    isSelected = false;
                    foreach (var item in claims)
                    {
                        if (claim.Type==item.Type)
                        {
                            isSelected = true;
                            break;
                        }
                       
                     
                    }
                    userClaim.Claims.Add(new UserClaim { ClaimType = claim.Type, IsSelected = isSelected });
                }
               

            }
           

            return View(userClaim);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaim(UserClaimViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"I cant find this user with id {model.Id}";
                return RedirectToAction("NotFound", "Administration");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            bool findClaim;


           foreach (var claim in model.Claims)
           {
                findClaim = false;
                foreach (var item in claims)
                {
                    if (claim.ClaimType == item.Type)
                    {
                        findClaim = true;
                        break;
                    }
                


                }
              if (!findClaim && claim.IsSelected)
         
                    {
                        Claim c1 = new Claim(claim.ClaimType, claim.ClaimType);

                        var result = await _userManager.AddClaimAsync(user, c1);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);

                            }
                            return View(model);
                        }
                    }

                else if (findClaim && !claim.IsSelected)
                {
                   
                
                        Claim c1 = new Claim(claim.ClaimType, claim.ClaimType);
                        var result = await _userManager.RemoveClaimAsync(user, c1);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);

                            }
                            return View(model);
                        }
                    
                   
                  
                }
            }

            return RedirectToAction("EditUser","Administration",new {id = user.Id });
        }
        public async Task<IActionResult> IsRoleExists(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role==null)
            {
                return Json(true);
            }
            else
            {
                return Json($"this role {roleName} is already In Use");
            }
        }


       
        //[AcceptVerbs("GET","POST")]
        //public async Task<IActionResult> IsEmailExists(string email)
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    foreach (var _user in users)
        //    {
        //        if (_user.Email == email)
        //        {
        //            return Json("This Email Already Exists");

        //        }

        //    }

        //    return Json(true);
        //}
    }
}