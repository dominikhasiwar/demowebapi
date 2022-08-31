using AutoMapper;
using DemoWebApi.Entities;
using DemoWebApi.Exceptions;
using DemoWebApi.Models;
using DemoWebApi.Providers;
using FluentValidation;

namespace DemoWebApi.Managers
{
    public interface IUserManager
    {
        Task<UserModel[]> GetUsers();

        Task<UserModel> GetUser(int userId);

        Task<UserModel> CreateUser(SaveUserModel model);

        Task<UserModel> UpdateUser(int userId, SaveUserModel model);

        void DeleteUser(int userId);
    }

    public class UserManager : IUserManager
    {
        private readonly IMapper _mapper;
        private readonly List<User> _users;

        public UserManager(IMapper mapper)
        {
            _mapper = mapper;
            _users = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Homer",
                    LastName = "Simpson",
                    UserName = "homer.simpson"
                }
            };
        }

        public Task<UserModel[]> GetUsers()
        {
            return Task.FromResult(_users.Select(_mapper.Map<UserModel>).ToArray());
        }

        public Task<UserModel> GetUser(int userId)
        {
            var user = _users.FirstOrDefault(x => x.Id == userId);

            if(user == null)
            {
                throw new NotFoundException($"User with id '{userId}' does not exist.");
            }

            return Task.FromResult(_mapper.Map<UserModel>(user));
        }

        public async Task<UserModel> CreateUser(SaveUserModel model)
        {
            var user = _mapper.Map<User>(model);

            user.Id = _users.Max(x => x.Id) + 1;
            _users.Add(user);

            return await GetUser(user.Id);
        }

        public async Task<UserModel> UpdateUser(int userId, SaveUserModel model)
        {
            var user = _users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"User with id '{userId}' does not exist.");
            }

            _mapper.Map(model, user);

            return await GetUser(userId);
        }

        public void DeleteUser(int userId)
        {
            _users.RemoveAll(x => x.Id == userId);
        }
    }
}
