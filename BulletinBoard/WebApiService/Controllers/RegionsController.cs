using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AppServices.Interfaces;

namespace WebApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        public RegionsController(IRegionService _regionService)
        {
            regionService = _regionService;
        }
        protected readonly IRegionService regionService;

        // GET api/values
        [HttpGet("get-regions-dictionary/{culture}")]
        public ActionResult<Dictionary<int, string>> GetAllRegions(string culture = "en")
        {
            Dictionary<int, string> regions = regionService.AllRegions(culture);
            return Ok(regions);
        }

        // GET api/values
        [HttpGet("get-region-name-by-id/{id}")]
        public ActionResult<string> GetRegionName(int id)
        {
            string categoryName = regionService.GetRegion(id).Name;
            return Ok(categoryName);
        }
    }
}
