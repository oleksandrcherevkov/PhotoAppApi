using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoAppApi.DAL.Generic;
using PhotoAppApi.EF;
using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.GeneralOptions;
using PhotoAppApi.Services.Photos;
using PhotoAppApi.Services.Photos.PostPhotos;
using PhotoAppApi.Services.Posts.Models;
using PhotoAppApi.Services.Posts.QueryObjects;
using PhotoAppApi.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Posts
{
    public class PostService : ServiceErrors 
    {
        private readonly IGenericRepository<Post> _postsRepo;
        private readonly IGenericRepository<PostUserView> _viewsRepo;
        private readonly PhotoService<PostPhoto> _photoService;
        private readonly UserService _userService;

        private readonly string _hostName;

        private readonly int _photoMaxSize;
        public PostService(IGenericRepository<Post> postsRepo, IGenericRepository<PostUserView>  viewsRepo, 
            UserService userService, PhotoService<PostPhoto> photoService, 
            IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _postsRepo = postsRepo;
            _viewsRepo = viewsRepo;
            _photoService = photoService;
            _userService = userService;

            _hostName = httpContextAccessor.HttpContext.Request.Host.Host;

            var filesSection = config.GetSection("Files");
            _photoMaxSize = filesSection.GetValue<int>("PostPhotoMaxSize");
        }

        public async Task<int> AddAsync(PostAddDto postInfo, string currentUserLogin)
        {
            var post = new Post()
            {
                CreatorLogin = currentUserLogin,
                Title = postInfo.Title,
                Description = postInfo.Description,
                CreationTime = DateTime.UtcNow
            };

            var transaction = await _postsRepo.BeginTransactionAsync();
            try
            {
                await _postsRepo.CreateAsync(post);
            }
            catch (Exception)
            {
                AddError($"Error occured during adding post with TITLE [{post.Title}].", nameof(post.Title));
            }
            

            var photoAdd = new PostPhotoAddWrap(postInfo.File, _photoMaxSize, post.Id);
            await _photoService.UploadAsync(photoAdd);

            if (_photoService.HasErrors)
            {
                AddErrors(_photoService.Errors);
                return 0;
            }

            await transaction.CommitAsync();
            return post.Id;
        }

        public async Task<PostListDto> ReadPostAsync(int id, string currentUserLogin)
        {
            var post = await _postsRepo.Read(p => p.Id == id)
                .SelectListDto(currentUserLogin, _hostName)
                .FirstOrDefaultAsync();

            if(post == null)
            {
                AddError($"Post with ID [{id}] not found.", nameof(id));
                return null;
            }

            PostUserView view = new PostUserView()
            {
                PostId = post.Id,
                UserLogin = currentUserLogin,
            };

            try
            {
                await _viewsRepo.CreateAsync(view);
            }
            catch (Exception)
            {
                // User has seen this post before.
            }

            return post;
        }

        public async Task<PostsPageDto> ListPostsAsync(PostsListOptions listOptions, string currentUserLogin, string login = default)
        {
            IQueryable<Post> postsQuery = _postsRepo.Read();

            if (!String.IsNullOrEmpty(login))
                postsQuery = postsQuery.Where(p => p.CreatorLogin == login);

            PostsPageDto page = await postsQuery
                .ReadPostsPagedAsync(listOptions, currentUserLogin, _hostName, compressedPhoto: true);

            return page;
        }

        public async Task DeleteAsync(int id, string currentUserLogin)
        {
            Post post = await _postsRepo.FindByIdAsync(id);

            if (post != null)
            {
                if (post.CreatorLogin == currentUserLogin)
                {
                    try
                    {
                        await _postsRepo.RemoveAsync(post);
                    }
                    catch (Exception)
                    {
                        AddError($"Error occured during deleting post with ID [{id}].", nameof(id));
                    }
                }
                else
                    AddError($"You are not permitted to delete post with ID [{id}].", nameof(id));
            }
            else
                AddError($"Post with ID [{id}] not found.", nameof(id));

        }

        public async Task LikeAsync(int id, string login)
        {
            var post = await _postsRepo
                .FindWithInclude(p => p.Id == id, p => p.Liked)
                .FirstOrDefaultAsync();

            if (post == null)
            {
                AddError($"Post with ID [{id}] not found.", nameof(id));
                return;
            }

            var user = await _userService.FindAsync(login);

            if (user == null)
            {
                AddError($"User with LOGIN [{login}] not found.", nameof(login));
                return;
            }

            if (post.Liked.Contains(user))
            {
                post.Liked.Remove(user);
                post.LikesCount--;
            }
            else
            {
                post.Liked.Add(user);
                post.LikesCount++;
            }

            try
            {
                await _postsRepo.UpdateAsync(post);
            }
            catch (Exception)
            {
                AddError($"Error occured during adding like of user with LOGIN [{login}]" +
                    $" to post with ID [{id}].", nameof(id), nameof(login));
            }
        }
    }
}
