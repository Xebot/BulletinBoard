using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BulletinDomain.Entities;
using WebApi.Contracts.DTO;

namespace BulletinDomain.MapperProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<AdvertDto, Advert>()  .ForMember(d=>d.Id, o=>o.MapFrom(s=>s.Id))
                                            .ForMember(d=>d.AdvertNumber, o=>o.MapFrom(s=>s.AdvertNumber))
                                            .ForMember(d=>d.UserId, o=>o.MapFrom(s=>s.UserId))
                                            .ForMember(d=>d.CategoryId, o=>o.MapFrom(s=>s.CategoryId))                                      
                                            .ForMember(d => d.RegionId, o => o.MapFrom(s => s.RegionId))
                                            .ForMember(d => d.AdvertText, o => o.MapFrom(s => s.AdvertText))
                                            .ForMember(d => d.Comments, o => o.MapFrom(s => s.Comments))
                                            .ForMember(d=>d.Status, o=>o.MapFrom(s=>s.Status))
                                            .ForMember(d=>d.PublicationDate, o=>o.MapFrom(s=>s.PublicationDate))
                                            .ForMember(d=>d.Title, o=>o.MapFrom(s=>s.Title))
                                            .ForMember(d => d.FolderName, o => o.MapFrom(s => s.FolderName))
                                            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s => s.PrimaryImageUrl))
                                            .ForMember(d => d.ImagesUrl, o => o.MapFrom(s => s.ImagesUrl))
                                            .ForMember(d => d.ShortDescription, o => o.MapFrom(s => s.ShortDescription))
                                            .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                                            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                                            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                                            .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone))
                                            .ForMember(a => a.Price, o => o.MapFrom(s => s.Price))                                            
                                            .ForAllOtherMembers(x => x.Ignore());
            CreateMap<Advert, AdvertDto>();

            CreateMap<CategoryDto, IQuarable>().ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                                            .ForMember(d => d.RuCategoryName, o => o.MapFrom(s => s.CategoryName))
                                            .ForMember(d => d.EnCategoryName, o => o.MapFrom(s => s.CategoryName))
                                            .ForMember(d=>d.CategoryUrl, o=>o.MapFrom(s=>s.CategoryUrl))
                                            .ForMember(d => d.ParentId, o => o.MapFrom(s => s.GroupId))
                                            .ForMember(d => d.Adverts, o => o.MapFrom(s => s.Adverts))
                                            .ForAllOtherMembers(x => x.Ignore());
            CreateMap<IQuarable, CategoryDto>();

            CreateMap<RegionDto, Region>().ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                                            .ForMember(d => d.RuName, o => o.MapFrom(s => s.Name))
                                            .ForMember(d => d.EnName, o => o.MapFrom(s => s.Name))
                                            .ForAllOtherMembers(x => x.Ignore());
            CreateMap<Region, RegionDto>();

            CreateMap<CommentDto, Comment>().ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                                            .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                                            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                                            .ForMember(d => d.UserAvatarUrl, o => o.MapFrom(s => s.UserAvatarUrl))
                                            .ForMember(d => d.ParentId, o => o.MapFrom(s => s.ParentId))
                                            .ForMember(d => d.AdvertId, o => o.MapFrom(s => s.AdvertId))
                                            .ForMember(d => d.CommentText, o => o.MapFrom(s => s.CommentText))
                                            .ForMember(d => d.PublicationDate, o => o.MapFrom(s => s.PublicationDate))
                                            .ForMember(d => d.CommentLikers, o => o.MapFrom(s => s.CommentLikers))
                                            .ForAllOtherMembers(x => x.Ignore());
            CreateMap<Comment, CommentDto>();

            CreateMap<CommentLikerDto, CommentLiker>().ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                                            .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                                            .ForMember(d => d.CommentId, o => o.MapFrom(s => s.CommentId))
                                            .ForMember(d => d.PublicationDate, o => o.MapFrom(s => s.PublicationDate))
                                            .ForAllOtherMembers(x => x.Ignore());
            CreateMap<CommentLiker, CommentLikerDto>();

            CreateMap<User, UserDto>().ForMember(d => d.UserTel, o => o.MapFrom(s => s.PhoneNumber))
                                      .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.Email))
                                      .ForMember(d => d.Id, o => o.MapFrom(s=> s.Id))
                                      .ForMember(d => d.UserAdress, o => o.MapFrom(s => s.UserAdress))
                                      .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                                      .ForMember(d => d.RegionId, o => o.MapFrom(s => s.RegionId))
                                      .ForMember(d => d.FIO, o => o.MapFrom(s => s.FIO))
                                      .ForMember(d => d.UserStatus, o => o.MapFrom(s => s.UserStatus));
            CreateMap<UserDto, User>().ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                                      .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.UserTel))
                                      .ForMember(d => d.Email, o=> o.MapFrom(s => s.UserEmail))
                                      .ForMember(d => d.UserName, o=> o.MapFrom(s => s.UserName))
                                      .ForMember(d => d.FIO, o => o.MapFrom(s => s.FIO))
                                      .ForMember(d => d.UserStatus, o => o.MapFrom(s => s.UserStatus));


            CreateMap<CreateUserDto, UserDto>().ForMember(d => d.UserEmail, o => o.MapFrom(s => s.Email))
                                               .ForMember(d => d.UserAdress, o => o.MapFrom(s => s.UserAdress))
                                               .ForMember(d => d.UserRegion, o => o.MapFrom(s => s.RegionId));
            CreateMap<UserDto, CreateUserDto>();
            
            


        }
    }
}
