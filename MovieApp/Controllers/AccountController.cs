using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MovieApp.Data;
using MovieApp.Models;
using System.Threading.Tasks;
using MovieApp.Models.AccountViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MovieApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly MovieAppContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public AccountController(MovieAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Account.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    SignIn(user);

                    // Handle ReturnUrl
                    string redirect = "/Home/";
                    if (Request.Cookies.ContainsKey("ReturnUrl"))
                    {
                        redirect = Request.Cookies["ReturnUrl"];
                        Response.Cookies.Delete("ReturnUrl");
                    }

                    return Redirect(redirect);
                }

            }
            // If we got this far, something failed, redisplay form
            return View();
        }

        private async void SignIn(Account user)
        {
            HttpContext.Session.SetString("Type", user.Type.ToString());

            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (user.ProfileImageUrl != null)
            {
                claims.Add(new Claim(ClaimTypes.Uri, user.ProfileImageUrl));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10) // Session Expiration  (after 10 min)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            { 
                var isEmailAlreadyExists = _context.Account.Any(x => x.Email == model.Email);
                if (isEmailAlreadyExists)
                {
                    ViewData["Message"] = $"{model.Email} already exists";
                    return View();
                }

                // Create new account object
                Account newAccount = new Account()
                {
                    Username = model.Username,
                    Password = model.Password,
                    Email = model.Email,
                    Type = Account.UserType.Customer
                };

                if (model.ProfileImage != null)
                {

                    //upload files to wwwroot
                    string fileName = Path.GetFileNameWithoutExtension(model.ProfileImage.FileName);
                    string extension = Path.GetExtension(model.ProfileImage.FileName);
                    model.ProfileImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/img/accounts/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(fileStream);
                    }

                    newAccount.ProfileImageUrl = model.ProfileImageUrl;
                } 

 
                try
                {
                    // Add object to context db
                    _context.Add(newAccount);

                    // Trying to save object to db
                    int isSaved = await _context.SaveChangesAsync();

                    if (isSaved > 0)
                    {
                        ViewData["Message"] = "You have been successfully registered!";
                        return RedirectToAction("Login");
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Message"] = $"problem in saving user";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Sign the user out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
