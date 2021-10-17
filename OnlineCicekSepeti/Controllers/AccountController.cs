using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineCicekSepeti.Controllers
{
    public class AccountController : Controller
    {

        private readonly ICountryService _countryService;
        private readonly IUserService _userService;
        private readonly ICityService _cityService;


        public AccountController(ICountryService countryService, IUserService userService, ICityService cityService)
        {

            _countryService = countryService;
            _userService = userService;
            _cityService = cityService;

        }


        public IActionResult UserDetailEdit()
        {
            var query = _userService.GetQuery();
            var model = query.SingleOrDefault(u => u.UserName == User.Identity.Name);

            

            return View(model);

        }
        [HttpPost]
        public IActionResult UserDetailEdit(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var usersResult = _userService.Update(model);
                if (usersResult.Status == ResultStatus.Exception)
                    throw new Exception(usersResult.message);
               
            }

            TempData["SuccesPassword"] = "Şifre baraşıryla değiştirildi!";

            return View(model);



        }
        public IActionResult Register()
        {
            var countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name");
            var model = new UserModel();
            return View(model);

        }
        [HttpPost]
        public IActionResult Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Add(model);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.message);
                if (result.Status == ResultStatus.Succes)
                    return RedirectToAction("Login");
                ModelState.AddModelError("", result.message);
            }
            var countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", model.UserDetail.CountryId);
            var citiesResult = _cityService.GetCities(model.UserDetail.CountryId);
            if (citiesResult.Status == ResultStatus.Exception)
                throw new Exception(citiesResult.message);
            ViewBag.Cities = new SelectList(citiesResult.Data, "Id", "Name", model.UserDetail.CityId);
            return View(model);
        }
        public IActionResult Login()
        {
            var model = new UserLoginModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetQuery().SingleOrDefault(u => u.UserName == model.UserName && u.Password == model.Password && u.active);

                if (user == null)
                {

                    ViewBag.Message = "Kullanıcı Adı veya Şifre Hatalı !";
                    return View(model);
                }
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role , user.Role.Name),
                    new Claim(ClaimTypes.Sid , user.Id.ToString())

                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                return RedirectToAction("Index", "Home", new { UserId = user.Id });
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            //HttpContext.Session.Remove("cart");
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }

        public IActionResult AccessDenied()
        {
            return View();

        }

    }
}
