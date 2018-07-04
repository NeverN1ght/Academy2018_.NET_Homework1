using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Academy2018_.NET_Homework1.Entities;

namespace Academy2018_.NET_Homework1.Data
{
    public class DataQueries
    {
        private readonly List<User> _dataHierarchy;
        public DataQueries(List<User> dataHierarchy)
        {
            _dataHierarchy = dataHierarchy;
        }

        public List<(Post Post, int Comments)> GetCommentsUnderUserPosts(int userId)
        {
            return _dataHierarchy
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Posts)
                .Select(p => (Post: p, CommentsCount: p.Comments.Count))
                .ToList();
        }

        public List<Comment> GetCommentsWithSmallBody(int userId)
        {
            return _dataHierarchy
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Posts)
                .SelectMany(p => p.Comments)
                .Where(c => c.Body.Length < 50)
                .ToList();
        }

        public List<(int Id, string Name)> GetCompletedTodos(int userId)
        {
            return _dataHierarchy
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Todos)
                .Where(t => t.IsComplete)
                .Select(t => (Id: t.Id, Name: t.Name))
                .ToList();
        }

        public List<User> GetUsersAscWithTodosDesc()
        {
            return _dataHierarchy
                .Select(u => u)
                .OrderBy(u => u.Name)
                .Select(u => new User {
                    Id = u.Id,
                    Address = u.Address,
                    Comments = u.Comments,
                    Posts = u.Posts,
                    CreatedAt = u.CreatedAt,
                    Name = u.Name,
                    Avatar = u.Avatar,
                    Email = u.Email,
                    Todos = u.Todos
                        .OrderByDescending(t => t.Name.Length)
                        .ToList()
                }).ToList();
        }

        public (
            User User, 
            Post LastPost, 
            int LastPostCommentsCount, 
            int UncompletedTodosCount, 
            Post MostPopularPostByComments, 
            Post MostPopularPostByLikes
            ) GetUserStructure(int userId)
        {
            return _dataHierarchy
                .Where(u => u.Id == userId)
                .Select(u => (
                User: u,
                LastPost: u.Posts
                    .OrderByDescending(p => p.CreatedAt)
                    .First(),
                LastPostCommentsCount: u.Posts
                    .OrderByDescending(p => p.CreatedAt)
                    .First().Comments.Count,
                UncompletedTodosCount: u.Todos
                    .Count(t => t.IsComplete == false),
                MostPopularPostByComments: u.Posts
                    .OrderByDescending(p => p.Comments.Count(c => c.Body.Length > 80))
                    .First(),
                MostPopularPostByLikes: u.Posts
                    .OrderByDescending(p => p.Likes)
                    .First()
                )).Single();
        }

        public (
            Post Post, 
            Comment LongestComment, 
            Comment MostLikedComment, 
            int CommentsCountUnderBadPost
            ) GetPostStructure(int postId)
        {
            return _dataHierarchy
                .SelectMany(u => u.Posts)
                .Where(p => p.Id == postId)
                .Select(p => (
                    Post: p,
                    LongestComment: p.Comments
                        .Select(c => c)
                        .OrderByDescending(c => c.Body.Length)
                        .First(),
                    MostLikedComment: p.Comments
                        .Select(c => c)
                        .OrderByDescending(c => c.Likes)
                        .First(),
                    CommentsCountUnderBadPost: p.Comments
                        .Where(c => p.Likes == 0 || p.Body.Length < 80)
                        .Select(c => c)
                        .Count()                      
                    )).Single();
        }
    }
}
