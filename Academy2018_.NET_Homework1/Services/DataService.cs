using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Academy2018_.NET_Homework1.Entities;
using Newtonsoft.Json;

namespace Academy2018_.NET_Homework1.Services
{
    public class DataService
    {
        private readonly HttpClient _client;
        private string baseUrl = "https://5b128555d50a5c0014ef1204.mockapi.io/";

        public DataService(HttpClient client)
        {
            _client = client;
        }

        private async Task<List<T>> GetDataCollectionAsync<T>(string endpoint) where T : class 
        {
            return JsonConvert.DeserializeObject<List<T>>(
                await _client.GetStringAsync(baseUrl + endpoint));
        }

        public async Task<List<User>> GetDataHierarchyAsync()
        {
            var users = await GetDataCollectionAsync<User>("users");
            var posts = await GetDataCollectionAsync<Post>("posts");
            var comments = await GetDataCollectionAsync<Comment>("comments");
            var todos = await GetDataCollectionAsync<Todo>("todos");
            var addresses = await GetDataCollectionAsync<Address>("address");

            #region Binding collections
            var dataHierarchy = (from user in users
                                 join post in posts on user.Id equals post.UserId into postGroup
                                 join todo in todos on user.Id equals todo.UserId into todoGroup
                                 join comment in comments on user.Id equals comment.UserId into userCommentGroup
                                 join address in addresses on user.Id equals address.UserId
                                 select new User
                                 {
                                     Id = user.Id,
                                     CreatedAt = user.CreatedAt,
                                     Name = user.Name,
                                     Avatar = user.Avatar,
                                     Email = user.Email,
                                     Address = new Address
                                     {
                                         UserId = address.UserId,
                                         Id = address.Id,
                                         City = address.City,
                                         Country = address.City,
                                         Street = address.Street,
                                         Zip = address.Zip
                                     },
                                     Comments = (from innerUserComment in userCommentGroup
                                                 select new Comment
                                                 {
                                                     UserId = innerUserComment.UserId,
                                                     Id = innerUserComment.Id,
                                                     Body = innerUserComment.Body,
                                                     CreatedAt = innerUserComment.CreatedAt,
                                                     Likes = innerUserComment.Likes,
                                                     PostId = innerUserComment.PostId
                                                 }).ToList(),
                                     Todos = (from innerTodo in todoGroup
                                              select new Todo
                                              {
                                                  Id = innerTodo.Id,
                                                  CreatedAt = innerTodo.CreatedAt,
                                                  UserId = innerTodo.UserId,
                                                  Name = innerTodo.Name,
                                                  IsComplete = innerTodo.IsComplete
                                              }).ToList(),
                                     Posts = (from innerPost in postGroup
                                              join comment in comments on innerPost.Id equals comment.PostId into postCommentGroup
                                              select new Post
                                              {
                                                  Id = innerPost.Id,
                                                  CreatedAt = innerPost.CreatedAt,
                                                  UserId = innerPost.UserId,
                                                  Title = innerPost.Title,
                                                  Body = innerPost.Body,
                                                  Likes = innerPost.Likes,
                                                  Comments = (from innerPostComment in postCommentGroup
                                                              select new Comment
                                                              {
                                                                  Id = innerPostComment.Id,
                                                                  CreatedAt = innerPostComment.CreatedAt,
                                                                  UserId = innerPostComment.UserId,
                                                                  PostId = innerPostComment.PostId,
                                                                  Likes = innerPostComment.Likes,
                                                                  Body = innerPostComment.Body
                                                              }).ToList()
                                              }).ToList()
                                 }).ToList();
            #endregion

            return dataHierarchy;
        }
    }
}
