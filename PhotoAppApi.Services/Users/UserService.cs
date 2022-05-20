using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoAppApi.EF;
using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Users.Models;
using PhotoAppApi.Services.Users.QueryObjects;
using PhotoAppApi.Services.Photos;
using PhotoAppApi.Services.Photos.Avatars;
using PhotoAppApi.DAL.Generic;

namespace PhotoAppApi.Services.Users
{
    public class UserService : ServiceErrors
    {
        private readonly IGenericRepository<User> _usersRepo;
        private readonly IGenericRepository<Avatar> _avatarsRepo;
        private readonly PhotoService<Avatar> _photoService;
        private readonly string _hostName;

        private readonly int _photoMaxSize;

        public UserService(IGenericRepository<User> usersRepo, IGenericRepository<Avatar> avatarsRepo, 
            PhotoService<Avatar> photoService, 
            IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _usersRepo = usersRepo;
            _avatarsRepo = avatarsRepo;
            _photoService = photoService;

            _hostName = httpContextAccessor.HttpContext.Request.Host.Host;

            var filesSection = config.GetSection("Files");
            _photoMaxSize = filesSection.GetValue<int>("AvatarMaxSize");
        }

        public async Task AddAsync(UserRegisterDto userInfo)
        {
            var user = new User()
            {
                Login = userInfo.Login,
                Email = userInfo.Email,
                Password = userInfo.Password,
                RegistrationDate = DateTime.UtcNow,
                Role = "user"
            };

            try
            {
                await _usersRepo.CreateAsync(user);
            }
            catch (Exception)
            {
                if (_usersRepo.Read(u => u.Login == user.Login).Any())
                {
                    AddError( "This Login is already in use.", nameof(user.Login));
                }
                if (_usersRepo.Read(u => u.Email == user.Email).Any())
                {
                    AddError( "This Email is already in use.", nameof(user.Email));
                }
            }
        }


        public async Task ConfirmAsync(string login)
        {
            var user = await FindAsync(login);

            if(user == null)
            {
                AddError($"User with LOGIN {login} does not exist.", nameof(login));
                return;
            }

            if (!user.Confirmed)
            {
                user.Confirmed = true;
                await _usersRepo.UpdateAsync(user);
            }
        }

        public async Task<User> ReadConfirmedWithPasswordAsync(string login, string password)
        {
            return await _usersRepo
                .Read(u => u.Login == login && u.Password == password && u.Confirmed)
                .FirstOrDefaultAsync();
        }

        public async Task<Tout> ReadConfirmedAsync<Tout>(Func<IQueryable<User>, IQueryable<Tout>> select, string login)
        {
            var userQuery = _usersRepo
                .Read(u => u.Login == login && u.Confirmed);
            Tout userDto = await select(userQuery)
                .FirstOrDefaultAsync();
            return userDto;
        }

        public async Task ChangeAvatarAsync(IFormFile file, string login)
        {
            UserListDto user = await ReadConfirmedAsync(u => u.SelectListDto(_hostName), login);

            if (user == null)
            {
                AddError($"User with LOGIN {login} not found.", nameof(login));
                return;
            }

            var transaction = await _avatarsRepo.BeginTransactionAsync();
            if (!String.IsNullOrEmpty(user.Avatar))
            {
                int currentAvatarId = await _avatarsRepo
                    .Read(a => a.UserLogin == login)
                    .Select(a => a.Id)
                    .FirstOrDefaultAsync();

                await _photoService.DeleteAsync(currentAvatarId);

                if (_photoService.HasErrors)
                {
                    AddErrors(_photoService.Errors);
                    return;
                }
            }

            var uploadAvatar = new AvatarAddWrap(file, _photoMaxSize, login);
            await _photoService.UploadAsync(uploadAvatar);

            if (_photoService.HasErrors)
            {
                AddErrors(_photoService.Errors);
                return;
            }

            await transaction.CommitAsync();
        }

        public async Task<User> FindAsync(string login)
        {
            return await _usersRepo.FindByIdAsync(login);
        }
    }
}
