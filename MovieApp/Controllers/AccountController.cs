using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

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

        // GET: Movies
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Account.ToListAsync());
        }

        [Authorize]
        [Authorize(Roles = "Admin")]

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id == 1)
            {
                return RedirectToAction("AccessDenied", "Errors");
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Username,Password,Type,ProfileImageUrl")]  Account account)
        {
            

            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Account.FindAsync(id);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
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
