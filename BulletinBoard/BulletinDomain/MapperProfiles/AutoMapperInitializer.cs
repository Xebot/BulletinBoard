using AutoMapper;
using BulletinDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;

namespace BulletinDomain.MapperProfiles
{
    public static class AutoMapperInitializer
    {
        public static void Initialize()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreateUserDto, User>();
                config.CreateMap<User, CreateUserDto>();
                config.CreateMap<AdvertDto, Advert>();
                config.CreateMap<Advert, AdvertDto>();
            });
        }
    }
}
