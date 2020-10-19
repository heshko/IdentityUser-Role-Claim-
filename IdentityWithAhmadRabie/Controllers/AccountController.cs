using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityWithAhmadRabie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWithAhmadRabie.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManger;
        private readonly SignInManager<AppUser> _signIn;

        public AccountController(UserManager<AppUser> userManger, SignInManager<AppUser> signIn)
        {
            _userManger = userManger;
            _signIn = signIn;
        }

       

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register( InputModel input)
        {

           

            if (ModelState.IsValid)
            {

                AppUser user = new AppUser
                {
                    Email = input.Email,
                    UserName = input.Email,
                    City = input.City,
                    Street = input.Street,
                    Zipcode =input.ZibCode
                };
                var result =await _userManger.CreateAsync(user, input.Password);

                if (result.Succeeded)
                {
                    var userSiginIn = await _userManger.GetUserAsync(User);

                    if (userSiginIn != null)
                    {
                        if ((_signIn.IsSignedIn(User)) && (await _userManger.IsInRoleAsync(userSiginIn, "Admin") || await _userManger.IsInRoleAsync(userSiginIn, "Administration")))
                        {
                            return LocalRedirect("/Admin/Administration/ListUsers");//^-*//
                        }
                        
                    }
                   
                    await _signIn.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }

                    return View(input);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult LogIn()
        {

          
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> LogIn(InputLogIn input, string ReturnUrl)
        {
            //ReturnUrl = "https://www.google.com/"; its work hhhhhhhhhhhhhhh
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Email = input.Email,
                    
                };
                var logIn = await _signIn.PasswordSignInAsync(input.Email,input.Password,input.RememberMe,false);
                if (logIn.Succeeded)
                {
               
                    if (!string.IsNullOrEmpty(ReturnUrl)&& Url.IsLocalUrl(ReturnUrl))
                    {
                       
                            return Redirect(ReturnUrl); // LockalDirect() om jag vill inte använd islockal
                        
                       
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Your Email or password is not valid");
                    return View(input);
                }
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
          
           await _signIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
           

        }

    //    [AcceptVerbs("Get","Post")]
    //    public ActionResult EmailExist(string email)
    //    {
    //        var user = _userManger.FindByNameAsync(email);
    //        if (user==null)
    //        {
    //            return Json(true);
    //        }
    //        else
    //        {
    //            return Json("this eamil already is Use");
    //        }
    //    }

        [AcceptVerbs("GET","POST")]

        public async Task< IActionResult> EmailExist(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"This Email{email} Already Is Use");
            }
        }
        
        [AcceptVerbs("GET","POST")]

        public async Task< IActionResult> IsEmailExist(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user == null)
            {
             
                return Json(true);
            }
            else
            {
                return Json($"This Email{email} Already Is Use");
            }
        }
    }
}