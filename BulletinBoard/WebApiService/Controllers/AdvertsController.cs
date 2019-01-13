using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.DTO;
using AppServices.Interfaces;
using WebApi.Contracts.DTO.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Authentication.AppServices.Extensions;

namespace WebApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertsController : ControllerBase
    {
        public AdvertsController(IAdvertService _advertService, IAuthenticationService _authenticationService)
        {
            advertService = _advertService;
            authenticationService = _authenticationService;
        }
        protected readonly IAdvertService advertService;
        protected readonly IAuthenticationService authenticationService;

        // GET api/values        
        [HttpGet]        
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("SignInUserAsync")]
        public ActionResult<IEnumerable<string>> SignInUserAsync(string email, string password)
        {
            UserLoginDto user = new UserLoginDto
            {
                Email = email,
                Password = password
            };
            authenticationService.SignInUserAsync(user);
            return new string[] { "value1", "value2" };
        }
        // GET api/values
        
        [HttpGet("All")]        
        public ActionResult<IEnumerable<string>> GetAll()
        {
            IList<AdvertDto> adverts = advertService.GetAllAdverts();
            return Ok(adverts);
        }

        [HttpGet("AllAdmin/{pageNumber}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserAdvertsDto> GetAllAdmin(int pageNumber)
        {
            UserAdvertsDto adverts = advertService.GetAllAdvertsAdmin(pageNumber);
            return Ok(adverts);
        }

        // GET api/values
        [HttpGet("get-last-twelve-adverts")]
        public ActionResult<IEnumerable<string>> GetLastTwelveAdverts()
        {
            IList<AdvertDto> adverts = advertService.GetLastTwelveAdverts();
            if (adverts == null)
            {
                return NotFound();
            }
            return Ok(adverts);
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            AdvertDto advert = advertService.GetAdvertById(id);
            if (advert == null)
            {
                return NotFound();
            }
            return Ok(advert);
        }

        [HttpPost]
        [Route ("save")]
        public ActionResult<string> Update([FromBody] AdvertDto ad)
        {
            advertService.UpdateAdvert(ad);
            return Ok();
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<string> CreateAdvert([FromBody] AdvertDto newadvert)
        {
            advertService.CreateAdvert(newadvert);
            return Ok();
        }       

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("UserAdverts/{id}/{pageNumber}")]
        public ActionResult<UserAdvertsDto> GetUserAdverts(Guid id,int pageNumber)
        {
            return advertService.GetUserAdverts(id,pageNumber);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

               

        // DELETE api/values/5
        //[HttpDelete("{id}"),Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost]
        //[Route("del")]
        //private void Delete(Guid id)
        //{
        //    advertService.DeleteAdvert(id);
        //}

        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAdvert(Guid id)
        {
            var advert = advertService.GetAdvertById(id);
            if (advert == null)
            {
                return NotFound();
            }
            var userId = HttpContext.User.GetUserName();
            var role = HttpContext.User.IsInRole("Admin");
            if (userId != advert.UserName && !role)
            {
                return Forbid();
            }
            advertService.DeleteAdvert(id);
            return Ok();
        }
        [HttpDelete("total/{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAdvertTotal(Guid id)
        {
            var advert = advertService.GetAdvertById(id);
            if (advert == null)
            {
                return NotFound();
            }            
            var role = HttpContext.User.IsInRole("Admin");
            if (!role)
            {
                return Forbid();
            }
            advertService.DeleteAdvertTotal(id);
            return Ok();
        }

        [HttpPost("Unpublish/{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult UnpublishAdvert(Guid id)
        {
            var advert = advertService.GetAdvertById(id);
            if (advert == null)
            {
                return NotFound();
            }
            var userId = HttpContext.User.GetUserId();
            var role = HttpContext.User.IsInRole("Admin");
            if (userId != advert.UserId && !role)
            {
                return Forbid("Dont match ID for userId and advert.UserId");
            }
            advertService.UnpublishAdvert(id);
            return Ok();
        }


        [HttpPost("filter")]
        public FilteredDto GetFiltered([FromBody] FilterDto filter)
        {
            return advertService.GetFiltered(filter);
        }
    }
}
