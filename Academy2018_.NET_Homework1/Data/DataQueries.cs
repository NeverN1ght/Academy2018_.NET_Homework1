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

        public void GetCommentsUnderUserPosts(int userId)
        {
            var result = _dataHierarchy
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Posts)
                .Select(x => (Title: x, Comments: x.Comments.Count));

            foreach (var res in result)
            {
                Console.WriteLine($"'{res.Title}' has {res.Comments} comments");
            }
        }

        public void GetCommentsWithSmallBody(int userId)
        {
            var result = _dataHierarchy
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Posts)
                .SelectMany(c => c.Comments)
                .Where(c => c.Body.Length < 50);

            foreach (var res in result)
            {
                Console.WriteLine($"'{res.Body}' has {res.Body.Length} symbols");
            }
        }

        public void GetCompletedTodos(int userId)
        {
            var result = _dataHierarchy
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Todos)
                .Where(t => t.IsComplete == true)
                .Select(x => (Id: x.Id, Name: x.Name));

            foreach (var res in result)
            {
                Console.WriteLine($"{res.Id} | '{res.Name}'");
            }
        }

        public void GetUsersAscWithTodosDesc()
        {
            var result = _dataHierarchy
                .Select(u => u)
                .OrderBy(u => u.Name)
                .ThenByDescending(u => u.Todos.SelectMany(t => t.Name));

            foreach (var res in result)
            {
                Console.WriteLine($"{res.Name} | '{res.Name}'");
            }
        }

        //public GetUserStructure(int userId)
        //{
        //    var result = _dataHierarchy.Where(u => u.Id == userId).Select(u => (
        //        User: u,
        //        LastPost: u.Posts.OrderBy(p => p.CreatedAt).First(),
        //        LastPostCommentsCount: u.Posts.OrderBy(p => p.CreatedAt).First().Comments.Count,
        //        UncompletedTodos: u.Todos.Count(t => t.IsComplete == false),
        //        MostPopularPostByComments: u.Posts.Where(p => p.Comments.Max(c => c.Body.Length > 80)),
        //        MostPopularPostByLikes: u.Posts.Select(p => p).Where(p => p.Likes)
        //        ));
        //}

        //public GetPostStructure(int postId)
        //{
        //    var result = _dataHierarchy.Select(u => u).Where(p => p.Pos
        //}
    }
}
