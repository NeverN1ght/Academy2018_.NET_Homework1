using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Academy2018_.NET_Homework1.Entities;
using Academy2018_.NET_Homework1.Services;
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
            var data = new DataService(new HttpClient());

            var result = await data.GetDataHierarchyAsync();

            Console.ReadKey();
        }
    }
}
