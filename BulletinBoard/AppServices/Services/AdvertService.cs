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
using WebApi.Contracts.DTO.Filters;
using Microsoft.EntityFrameworkCore;

namespace AppServices.Services
{
    public class AdvertService : IAdvertService
    {
        public AdvertService(IAdvertRepository _advertRepository, IMapper _mapper)
        {
            advertRepository = _advertRepository;
            mapper = _mapper;
        }
        protected readonly IAdvertRepository advertRepository;
        protected readonly IMapper mapper;

        public Guid CreateAdvert(AdvertDto advertDto)
        {
            var result = mapper.Map<Advert>(advertDto);
            advertRepository.Create(result);
            return advertDto.Id;            
        }

        public void DeleteAdvert(Guid advertId)
        {            
            var ad = advertRepository.Get(advertId);
            if (ad.Status != "Deleted")
            {
                ad.Status = "Deleted";
            }
            else
            {
                ad.Status = "Unactive";
            }
            advertRepository.Update(ad);
        }
        public void DeleteAdvertTotal(Guid advertId)
        {
            advertRepository.Detele(advertId);            
        }

        public AdvertDto GetAdvertById(Guid advertId)
        {
            Advert ad = advertRepository.Get(advertId);            
            if (ad == null)
            {
                throw new ArgumentException($"Не найдено объявление с ID = {advertId}");
            }
            var result = mapper.Map<AdvertDto>(ad);
            return result;

        }
        public List<CommentDto> GetAllAdvertComments(Guid advertId)
        {
            var ad = advertRepository.Get(advertId);
            var result = mapper.Map<List<CommentDto>>(ad.Comments);
            return result;           
        }

        public void UpdateAdvert(AdvertDto advertDto)
        {
            var ad = mapper.Map<Advert>(advertDto);
            advertRepository.Update(ad);
        }
        public IList<AdvertDto> GetAllAdverts()
        {
            var ad = advertRepository.GetAll();
            List<Advert> adverts = null;
            if (ad!= null)
            {
                adverts = ad.ToList();
                var result = mapper.Map<List<AdvertDto>>(adverts);
                return result;
            }
            else
            {
                return null;
            }
            
        }

        public UserAdvertsDto GetAllAdvertsAdmin(int pageNumber)
        {
            var ad = advertRepository.GetAll();
            int skipedAdvertsNumber = 12 * (pageNumber - 1);
            var count = ad.Count();
            UserAdvertsDto userAdvertsDto = new UserAdvertsDto();
            userAdvertsDto.count = count;
            List<Advert> adverts = null;
            if (ad != null)
            {
                adverts = ad.Skip(skipedAdvertsNumber).Take(12).ToList();
                userAdvertsDto.ads = mapper.Map<List<AdvertDto>>(adverts);
                return userAdvertsDto;
            }
            else
            {
                return null;
            }

        }


        public FilteredDto GetFiltered(FilterDto filter) 
        {

            int skipedAdvertsNumber = filter.advertsPerPageNumber * (filter.PageNumber - 1);
            var query = advertRepository.GetAll();
            if (filter.PriceRange != null)
            {
                if (filter.PriceRange.MinV.HasValue)
                    query = query.Where(x=> x.Price >= filter.PriceRange.MinV);
                if (filter.PriceRange.MaxV.HasValue)
                    query = query.Where(x=> x.Price <= filter.PriceRange.MaxV);
            }
            if (!string.IsNullOrEmpty(filter.Category) && Convert.ToInt32(filter.Category)!=1 )
            {
                int i = Convert.ToInt32(filter.Category);
                query = query.Where(x => x.CategoryId == i || x.Category.ParentId == i);
            }                            
            //Данный фильтр ищет в коротком описании и в расширенном описании,
            //но не надо использовать его если стоит галочка "искать только в названии"
            if (!string.IsNullOrEmpty(filter.Text))
            {
                query = query.Where(x => EF.Functions.Like(x.ShortDescription.ToUpper(), $"%{filter.Text.ToUpper()}%")
                                        || EF.Functions.Like(x.Title.ToLower(), $"%{filter.Text.ToLower()}%")
                                        || EF.Functions.Like(x.Description.ToUpper(), $"%{filter.Text.ToUpper()}%"));               
            }
            ////Этот фильтр применяется во всех случаях поиска. Даже если стоит галочка "искать только в названии"
            if (!string.IsNullOrEmpty(filter.Title) && (string.IsNullOrEmpty(filter.Text)))
            {                
                query = query.Where(x => x.Title.ToLower().Contains(filter.Title.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.isActive) && string.IsNullOrEmpty(filter.Role))
            {
                query = query.Where(x => x.Status == "Published");
            }
            if (!string.IsNullOrEmpty(filter.Region))
            {
                int i = Convert.ToInt32(filter.Region);
                query = query.Where(x => x.RegionId == i);
            }
            if (filter.Pic == true)
            {
                query = query.Where(x => x.ImagesUrl != null);
            }
            var count = query.Count();
            var entities = query.Skip(skipedAdvertsNumber).Take(filter.advertsPerPageNumber).ToArray();
            var list = mapper.Map<AdvertDto[]>(entities);
            FilteredDto filt = new FilteredDto
            {
                adverts = list,
                TotalCount = count
            };
            return filt;
        }

        public IList<AdvertDto> GetLastTwelveAdverts()
        {
            var ad = advertRepository.GetAll().Where(z => z.Status == "Published").OrderByDescending(x => x.PublicationDate).Take(12).ToList();
            var result = mapper.Map<List<AdvertDto>>(ad);
            return result;
        }
        
        //public List<string> AllTitles()
        //{
        //    var list = advertRepository.GetAll().Select(x => x.Title).ToList();
        //    return list;
        //}

        public UserAdvertsDto GetUserAdverts(Guid UserId, int pageNumber)
        {

            UserAdvertsDto userAdvertsDto = new UserAdvertsDto();
            int skipedAdvertsNumber = 12 * (pageNumber - 1);
            
            var list = advertRepository.GetAll();
            int count = 0;
            
            if (list!= null)
            {
                var ads = list.Where(x => x.UserId == UserId && x.Status != "Deleted").ToList();
                count = ads.Count();
                if (pageNumber!= 0)
                {
                   var adverts = ads.Skip(skipedAdvertsNumber).Take(12);
                   userAdvertsDto.ads = mapper.Map<List<AdvertDto>>(adverts);

                }
                else
                {
                    var adverts = ads;
                    userAdvertsDto.ads = mapper.Map<List<AdvertDto>>(adverts);
                }
                
                
                userAdvertsDto.count = count;
                return userAdvertsDto;
            }
            else
            {
                return null;
            }
            
        }

        public void UnpublishAdvert(Guid UserId)
        {
            var advert = advertRepository.Get(UserId);
            if (advert.Status == "Unactive")
            {
                advert.Status = "Published";
            }
            else
            {
                advert.Status = "Unactive";
            }
            advertRepository.Update(advert);
        }

    }
}
