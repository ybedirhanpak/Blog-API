using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Dtos.Dtos.UserDtos;
using Blog_Project.Dtos.UserDtos;
using Blog_Project.Models;

namespace Blog_Project.Settings
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            //User
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserAuthenticatedDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserOutDto>();

            //Category
            CreateMap<CategoryInDto, Category>();

            //User-Category
            CreateMap<UserCategoryDto, UserCategory>();

            //Post
            CreateMap<PostInDto, Post>();
            CreateMap<Post, PostOutDto>();
            
            //Comment
            CreateMap<CommentCreateDto, Comment>();
        }

    }
}
