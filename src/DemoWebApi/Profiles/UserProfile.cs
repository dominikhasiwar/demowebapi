using AutoMapper;
using DemoWebApi.Entities;
using DemoWebApi.Models;

namespace DemoWebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>();

            CreateMap<SaveUserModel, User>();
        }
    }
}
