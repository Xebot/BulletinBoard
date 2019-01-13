using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;

namespace AppServices.Interfaces
{
    public interface IRegionService
    {
        Dictionary<int, string> AllRegions(string culture);
        RegionDto GetRegion(int id);
    }
}
