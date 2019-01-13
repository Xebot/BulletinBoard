using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;
using BulletinDomain;
using BulletinDomain.RepositoryInterfaces;
using AppServices.Interfaces;
using BulletinDomain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AppServices.Services
{
    public class CategoryService : ICategoryService
    {
        public CategoryService(ICategoryRepository _categoryRepository, IMapper _mapper)
        {
            categoryRepository = _categoryRepository;
            mapper = _mapper;
        }
        protected readonly ICategoryRepository categoryRepository;
        protected readonly IMapper mapper;

        public IList<CategoryDto> GetAll()
        {
            var categories = categoryRepository.GetAll();
            if (categories != null)
            {
                IList<CategoryDto> result = mapper.Map<List<CategoryDto>>(categories.ToList());
                return result;
            }
            return null;

            //IList<CategoryDto> result = mapper.Map<List<CategoryDto>>(categories);
            
        }

        public int GetCategoryIdByUrl(string categoryUrl)
        {
            int result = categoryRepository.Get(categoryUrl).Id;
            return result;
        }

        public string GetCategoryNameByUrl(string categoryUrl, string culture)
        {
            string result = "";
            if (culture == "ru")
            {
                result = categoryRepository.Get(categoryUrl).RuCategoryName;
            }
            else
            {
                result = categoryRepository.Get(categoryUrl).EnCategoryName;
            }
            return result;
        }

        public List<AdvertDto> GetCategoryAdvertsByUrl(string categoryUrl, int pageNumber, int advertsPerPageNumber)
        {
            int skipedAdvertsNumber = advertsPerPageNumber * (pageNumber - 1);     
            List<Advert> categoriesAdvertsList = new List<Advert>();
            int categoryId = categoryRepository.GetAll().Where(p => p.CategoryUrl == categoryUrl).FirstOrDefault().Id;
            List<int> subcategoriesIdList = categoryRepository.GetAll().Where(p => p.ParentId == categoryId).Select(p => p.Id).ToList();
            if (subcategoriesIdList.Count() != 0)
            {
                IQueryable<Advert> categoriesAdverts = (new List<Advert>()).AsQueryable();
                foreach (var subcategoryId in subcategoriesIdList)
                {
                    IQueryable<Advert> subcategoryAdverts = categoryRepository.GetAll().Include(p => p.Adverts).Where(p => p.Id == subcategoryId).FirstOrDefault().Adverts.Where(p => p.Status == "Published").AsQueryable();
                    categoriesAdverts = categoriesAdverts.Concat(subcategoryAdverts);
                }
                categoriesAdvertsList = categoriesAdverts.OrderBy(p => p.PublicationDate).Skip(skipedAdvertsNumber).Take(advertsPerPageNumber).ToList();
            }
            else
            {
                categoriesAdvertsList = categoryRepository.GetAll().Include(p => p.Adverts).Where(p => p.CategoryUrl == categoryUrl).FirstOrDefault().Adverts.Where(p => p.Status == "Published").OrderBy(p => p.PublicationDate).Skip(skipedAdvertsNumber).Take(advertsPerPageNumber).ToList();
            }
            var result = mapper.Map<List<AdvertDto>>(categoriesAdvertsList);
            return result;
        }

        public int GetCategoryAdvertsNumber(string categoryUrl) {
            int categoryAdvertsNumber = 0;
            int categoryId = categoryRepository.GetAll().Where(p => p.CategoryUrl == categoryUrl).FirstOrDefault().Id;
            List<int> subcategoriesIdList = categoryRepository.GetAll().Where(p => p.ParentId == categoryId).Select(p => p.Id).ToList();
            if (subcategoriesIdList.Count() != 0)
            {
                foreach (var subcategoryId in subcategoriesIdList)
                {
                    int subcategoryAdvertsNumber = categoryRepository.GetAll().Include(p => p.Adverts).Where(p => p.Id == subcategoryId).FirstOrDefault().Adverts.Where(p => p.Status == "Published").Count();
                    categoryAdvertsNumber += subcategoryAdvertsNumber;
                }                
            }
            else
            {
                categoryAdvertsNumber = categoryRepository.GetAll().Include(p => p.Adverts).Where(p => p.CategoryUrl == categoryUrl).FirstOrDefault().Adverts.Where(p => p.Status == "Published").Count();
            }
            return categoryAdvertsNumber;
        }

        public List<CategorySubcategories> GetCategoriesListWithSubcategories(string culture)
        {
            List<CategorySubcategories> result = new List<CategorySubcategories>();
            List<int> categoriesIdList = categoryRepository.GetAll().Where(x => x.ParentId == 0).Select(x => x.Id).ToList();

            var allCategories = categoryRepository.GetAll().ToList();

            for (int i = 0; i < categoriesIdList.Count(); i++)
            {
                string categoryName = "";
                if (culture == "ru")
                {
                    categoryName = allCategories.Where(x => x.Id == categoriesIdList[i]).FirstOrDefault().RuCategoryName;
                }
                if (culture == "en")
                {
                    categoryName = allCategories.Where(x => x.Id == categoriesIdList[i]).FirstOrDefault().EnCategoryName;
                }
                Dictionary<int, string> subcategoriesIdAndNameDictionary = new Dictionary<int, string>();                
                
                List<int> subcategoriesIdList = allCategories.Where(x => x.ParentId == categoriesIdList[i]).Select(x => x.Id).ToList();
                for (int j = 0; j < subcategoriesIdList.Count(); j++)
                {
                    string subcategoryName = "";
                    if (culture == "ru")
                    {
                        subcategoryName = allCategories.Where(x => x.Id == subcategoriesIdList[j]).FirstOrDefault().RuCategoryName;
                    }
                    else
                    {
                        subcategoryName = allCategories.Where(x => x.Id == subcategoriesIdList[j]).FirstOrDefault().EnCategoryName;
                    }
                    subcategoriesIdAndNameDictionary.Add(subcategoriesIdList[j], subcategoryName);
                }
                result.Add(new CategorySubcategories { CategoryId = categoriesIdList[i], CategoryName = categoryName, Subcategories = subcategoriesIdAndNameDictionary });
            }
            return result;
        }      
    
        


        public List<string> GetSubcategoriesUrlListByCategoryUrl(string categoryUrl)
        {
            int categoryId = GetCategoryIdByUrl(categoryUrl);            
            List<string> subcategoriesUrlList = categoryRepository.GetAll().Where(x => x.ParentId == categoryId).Select(x => x.CategoryUrl).ToList();
            List<string> result = subcategoriesUrlList.Count() == 0 ? null : subcategoriesUrlList;
            return result;
        }

        public Dictionary<string, string> GetSubcategoriesNameAndUrlDictionary(string categoryUrl, string culture)
        {
            int categoryId = GetCategoryIdByUrl(categoryUrl);            
            List<IQuarable> subcategoriesList = categoryRepository.GetAll().Where(x => x.ParentId == categoryId).ToList();
            Dictionary<string, string> subcategoriesNameAndUrlDictionary = new Dictionary<string, string>();
            if (subcategoriesList.Count() != 0)
            {
                if (culture == "ru")
                {
                    foreach (var subcategory in subcategoriesList)
                    {
                        subcategoriesNameAndUrlDictionary.Add(subcategory.RuCategoryName, subcategory.CategoryUrl);
                    }
                }
                else
                {
                    foreach (var subcategory in subcategoriesList)
                    {
                        subcategoriesNameAndUrlDictionary.Add(subcategory.EnCategoryName, subcategory.CategoryUrl);
                    }
                }
            }
            else
            {
                subcategoriesNameAndUrlDictionary = null;
            }
            return subcategoriesNameAndUrlDictionary;
        }

        public List<CategoryDto> GetAllCategories()
        {
            var ad = categoryRepository.GetAll().ToList();
            var result = mapper.Map<List<CategoryDto>>(ad);
            return result;
        }

    }
}
