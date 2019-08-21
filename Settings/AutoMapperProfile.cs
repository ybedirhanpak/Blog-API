using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Models;

namespace Blog_Project.Settings
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            //User
            CreateMap<UserInDto, User>();

            //Category
            CreateMap<CategoryInDto, Category>();

            //Post
            CreateMap<PostInDto, Post>();
            CreateMap<Post, PostOutDto>();

        }

    }
}
