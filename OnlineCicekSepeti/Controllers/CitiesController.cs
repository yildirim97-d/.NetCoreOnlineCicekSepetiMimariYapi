using AppCore.Business.Models.Results;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCicekSepeti.Controllers
{
    [Route("[controller]")]
    public class CitiesController : Controller
    {
        private readonly ICityService _cityServices;
        public CitiesController(ICityService cityServices)
        {
            _cityServices = cityServices;
        }
        [Route("CitiesGet/{countryId?}")]
        public IActionResult GetCitiesCountryIdWithGet(int? countryId)
        {
            if (countryId == null)
                return View("NotFound");
            var result = _cityServices.GetCities(countryId.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.message);
            

            return Json(result.Data);
        }
    }
}
