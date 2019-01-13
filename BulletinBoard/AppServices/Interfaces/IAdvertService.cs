using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Contracts.DTO;
using WebApi.Contracts.DTO.Filters;

namespace AppServices.Interfaces
{
    public interface IAdvertService
    {
        List<CommentDto> GetAllAdvertComments(Guid advertId);
        AdvertDto GetAdvertById(Guid advertId);
        Guid CreateAdvert(AdvertDto advertDto);
        void UpdateAdvert(AdvertDto advertDto);
        void DeleteAdvert(Guid advertId);
        void DeleteAdvertTotal(Guid advertId);
        IList<AdvertDto> GetAllAdverts();
        //IList<AdvertDto> GetCategoryAdverts(int categoryId);
        UserAdvertsDto GetAllAdvertsAdmin(int pageNumber);
        FilteredDto GetFiltered(FilterDto filter);
        IList<AdvertDto> GetLastTwelveAdverts();
       // List<string> AllTitles();
        UserAdvertsDto GetUserAdverts(Guid UserId, int pageNumber);
        void UnpublishAdvert(Guid UserId);
    }
}
