using System;
using System.Collections.Generic;
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
            var result = await client.GetStringAsync("https://5b128555d50a5c0014ef1204.mockapi.io/users");
            var list = JsonConvert.DeserializeObject<List<User>>(result);
            foreach (var l in list)
            {
                Console.WriteLine(l.Id + " | " + l.Name + " | " + l.CreatedAt);
            }

            Console.ReadKey();
        }
    }
}
