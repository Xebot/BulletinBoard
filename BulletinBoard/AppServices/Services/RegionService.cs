using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;
using BulletinDomain;
using BulletinDomain.RepositoryInterfaces;
using AppServices.Interfaces;
using BulletinDomain.Entities;
using System.Linq;
using AutoMapper;

namespace AppServices.Services
{
    public class RegionService : IRegionService
    {
        public RegionService(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        protected readonly IRegionRepository _regionRepository;
        protected readonly IMapper _mapper;

        public Dictionary<int, string> AllRegions(string culture)
        {
            var regionsList = _regionRepository.GetAll().ToList();
            Dictionary<int, string> regionsDictionary = new Dictionary<int, string>();
            if (regionsList.Count() != 0)
            {
                if (culture == "ru")
                {
                    foreach (var region in regionsList)
                    {
                        regionsDictionary.Add(region.Id, region.RuName);
                    }
                }
                else
                {
                    foreach (var region in regionsList)
                    {
                        regionsDictionary.Add(region.Id, region.EnName);
                    }
                }
            }
            else
            {
                regionsDictionary = null;
            }
            var result = _mapper.Map<Dictionary<int, string>>(regionsDictionary);
            return result;
        }

        public RegionDto GetRegion(int id)
        {
            var region = _regionRepository.Get(id);
            var result = _mapper.Map<RegionDto>(region);
            return result;
        }
    }
}
