using Lab28Tom.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab28Tom.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email, DestinyBirthday = rvm.DestinyBirthday, PowerLevel = rvm.PowerLevel };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                //if user was successfully registered
                if (result.Succeeded)
                {
                    const string issuer = "www.destiny.com";
                    List<Claim> myClaims = new List<Claim>();

                    //power level claim
                    Claim power = new Claim(ClaimTypes.Role, rvm.PowerLevel.ToString(), ClaimValueTypes.Integer32, issuer);
                    myClaims.Add(power);

                    Claim dbirthday = new Claim(ClaimTypes.DateOfBirth, rvm.DestinyBirthday.Date.ToString(), ClaimValueTypes.Date);
                    myClaims.Add(dbirthday);

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    var addClaims = await _userManager.AddClaimsAsync(user, myClaims);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult AdminRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminRegister(RegisterViewModel arvm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = arvm.Email, Email = arvm.Email };
                var result = await _userManager.CreateAsync(user, arvm.Password);

                if (result.Succeeded)
                {
                    //Create a list where my claims will be added to
                    List<Claim> myClaims = new List<Claim>();

                    // claim for the User's role
                    Claim claim1 = new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String);
                    myClaims.Add(claim1);

                    var addClaims = await _userManager.AddClaimsAsync(user, myClaims);

                    if (addClaims.Succeeded)
                    {
                        await _signInManager.PasswordSignInAsync(arvm.Email, arvm.Password, true, lockoutOnFailure: false);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View("Forbidden");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(lvm.Email);

                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    const string issuer = "www.destiny.com";

                    List<Claim> myClaims = new List<Claim>();

                    //power level claim
                    Claim power = new Claim(ClaimTypes.Name, user.PowerLevel.ToString(), ClaimValueTypes.Integer32, issuer);
                    myClaims.Add(power);

                    Claim dbirthday = new Claim(ClaimTypes.DateOfBirth, user.DestinyBirthday.Date.ToString(), ClaimValueTypes.Date);
                    myClaims.Add(dbirthday);

                    var userIdentity = new ClaimsIdentity("Registration");
                    userIdentity.AddClaims(myClaims);

                    var userPrinciple = new ClaimsPrincipal(userIdentity);

                    User.AddIdentity(userIdentity);

                    await HttpContext.SignInAsync(
                    "MyCookieLogin", userPrinciple,
                        new AuthenticationProperties
                        {
                             ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                            IsPersistent = false,
                            AllowRefresh = false

                        });

                    return RedirectToAction("Index", "Home");
                }

            }
            string error = "Incorrect username or password";
            ModelState.AddModelError("", error);
            return View();
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return View();
        }
    }
}
