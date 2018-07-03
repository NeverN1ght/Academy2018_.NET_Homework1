using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Academy2018_.NET_Homework1.Entities;
using Newtonsoft.Json;

namespace Academy2018_.NET_Homework1
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            var client = new HttpClient();
            var users = JsonConvert.DeserializeObject<List<User>>(
                await client.GetStringAsync("https://5b128555d50a5c0014ef1204.mockapi.io/users"));
            var posts = JsonConvert.DeserializeObject<List<Post>>(
                await client.GetStringAsync("https://5b128555d50a5c0014ef1204.mockapi.io/posts"));
            var comments = JsonConvert.DeserializeObject<List<Comment>>(
                await client.GetStringAsync("https://5b128555d50a5c0014ef1204.mockapi.io/comments"));
            var todos = JsonConvert.DeserializeObject<List<Todo>>(
                await client.GetStringAsync("https://5b128555d50a5c0014ef1204.mockapi.io/todos"));

            var result = (from user in users
                join post1 in posts on user.Id equals post1.UserId into postGroup
                join todo in todos on user.Id equals todo.UserId into todoGroup
                select new User {
                    Id = user.Id,
                    CreatedAt = user.CreatedAt,
                    Name = user.Name,
                    Address = user.Address,
                    Avatar = user.Avatar,
                    Email = user.Email,
                    Todos = (from todo2 in todoGroup
                        select new Todo {
                            Id = todo2.Id,
                            CreatedAt = todo2.CreatedAt,
                            UserId = todo2.UserId,
                            Name = todo2.Name,
                            IsComplete = todo2.IsComplete
                        }).ToList(),
                    Posts = (from post2 in postGroup
                        join comment in comments on post2.Id equals comment.PostId into commentGroup
                        select new Post {
                            Id = post2.Id,
                            CreatedAt = post2.CreatedAt,
                            UserId = post2.UserId,
                            Title = post2.Title,
                            Body = post2.Body,
                            Likes = post2.Likes,
                            Comments = (from comment2 in commentGroup
                                    select new Comment {
                                        Id = comment2.Id,
                                        CreatedAt = comment2.CreatedAt,
                                        UserId = comment2.UserId,
                                        PostId = comment2.PostId,
                                        Likes = comment2.Likes,
                                        Body = comment2.Body
                                    }).ToList()
                        }).ToList()
                }).ToList();



            foreach (var r in result)
            {
                Console.WriteLine($"{r.Id} | {r.Name} has posts {r.Posts}");
            }

            Console.ReadKey();
        }
    }
}
