using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Dtos.CategoryDtos;
using Blog_Project.Dtos.CommentDtos;
using Blog_Project.Dtos.PostDtos;
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
            CreateMap<User, UserOutDto>();
            CreateMap<UserFollow, UserFollowOutDto>();

            //Main Category
            CreateMap<MainCategoryCreateDto, MainCategory>();
            CreateMap<MainCategory, MainCategoryOutDto>();

            //Category
            CreateMap<CategoryInDto, Category>();
            CreateMap<Category, CategoryOutDto>();

            //User-Category
            CreateMap<UserCategory, UserCategoryOutDto>();

            //User-Post
            CreateMap<UserLikePost, UserLikePostOutDto>();

            //Post
            CreateMap<PostCreateDto, Post>();
            CreateMap<Post, PostOutDto>();
            
            //Comment
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<Comment, CommentOutDto>();

        }

    }
}
