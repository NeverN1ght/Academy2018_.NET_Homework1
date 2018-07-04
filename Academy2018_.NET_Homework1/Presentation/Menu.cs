using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Academy2018_.NET_Homework1.Data;
using Academy2018_.NET_Homework1.Entities;
using Academy2018_.NET_Homework1.Services;

namespace Academy2018_.NET_Homework1.Presentation
{
    public static class Menu
    {
        private static bool _isExit = false;

        public static async Task Run()
        {
            var data = new DataService(new HttpClient());
            var queries = new DataQueries(await data.GetDataHierarchyAsync());

            while (!_isExit)
            {
                Console.WriteLine("Запросы: ");
                Console.WriteLine(" 1 - Получить количество комментов под постами конкретного пользователя (по айди) (список из пост-количество)");
                Console.WriteLine(" 2 - Получить список комментов под постами конкретного пользователя (по айди), где body коммента < 50 символов (список из комментов)");
                Console.WriteLine(" 3 - Получить список (id, name) из списка todos которые выполнены для конкретного пользователя (по айди)");
                Console.WriteLine(" 4 - Получить список пользователей по алфавиту (по возрастанию) с отсортированными todo items по длине name (по убыванию)");
                Console.WriteLine(" 5 - Получить структуру пользователя (передать Id пользователя в параметры)");
                Console.WriteLine(" 6 - Получить структуру поста (передать Id поста в параметры)");
                Console.WriteLine(" 7 - Выход");
                Console.Write("Выберите действие: ");

                int choice = 0;
                int id;

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Введите id пользователя: ");
                        id = int.Parse(Console.ReadLine());
                        var query1Result = queries.GetCommentsUnderUserPosts(id);
                        Console.WriteLine("=========================================");
                        if (query1Result.Count == 0)
                        {
                            Console.WriteLine("Данных нет :(");
                        }
                        else
                        {
                            foreach (var record in query1Result)
                            {
                                Console.WriteLine($"Пост \"{record.Post.Title}\" имеет {record.Comments} коммент");
                            }
                        }
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        Console.Write("Введите id пользователя: ");
                        id = int.Parse(Console.ReadLine());
                        var query2Result = queries.GetCommentsWithSmallBody(id);
                        Console.WriteLine("=========================================");
                        if (query2Result.Count == 0)
                        {
                            Console.WriteLine("Данных нет :(");
                        }
                        else
                        {
                            foreach (var record in query2Result)
                            {
                                Console.WriteLine($"Коммент \"{record.Body}\" имеет {record.Body.Length} символов");
                            }
                        }
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 3:
                        Console.Write("Введите id пользователя: ");
                        id = int.Parse(Console.ReadLine());
                        var query3Result = queries.GetCompletedTodos(id);
                        Console.WriteLine("=========================================");
                        if (query3Result.Count == 0)
                        {
                            Console.WriteLine("Данных нет :(");
                        }
                        else
                        {
                            foreach (var record in query3Result)
                            {
                                Console.WriteLine($"Id: {record.Id} | Таска \"{record.Name}\" выполнена");
                            }
                        }
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
                        var query4Result = queries.GetUsersAscWithTodosDesc();
                        Console.WriteLine("=========================================");
                        if (query4Result.Count == 0)
                        {
                            Console.WriteLine("Данных нет :(");
                        }
                        else
                        {
                            foreach (var record in query4Result)
                            {
                                Console.WriteLine($"Имя: {record.Name} \n{GetTodosToShow(record.Todos)}");
                            }
                        }
                        Console.WriteLine("=========================================");                 
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 5:
                        Console.Write("Введите id пользователя: ");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine("=========================================");
                        try
                        {
                            var query5Result = queries.GetUserStructure(id);
                            Console.WriteLine($"Пользователь: {query5Result.User.Name} \n" +
                                              $"Последний пост пользователя (по дате): \"{query5Result.LastPost.Title}\"\n" +
                                              $"Количество комментов под последним постом: {query5Result.LastPostCommentsCount} \n" +
                                              $"Количество невыполненных тасков для пользователя: {query5Result.UncompletedTodosCount} \n" +
                                              $"Самый популярный пост пользователя (там где больше всего комментов с длиной текста больше 80 символов): \"{query5Result.MostPopularPostByComments.Title}\"\n" +
                                              $"Самый популярный пост пользователя (там где больше всего лайков): \"{query5Result.MostPopularPostByLikes.Title}\"\n");
                        }
                        catch (InvalidOperationException)
                        {
                            Console.WriteLine("Данных нет :(");
                        }                     
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 6:
                        Console.Write("Введите id поста: ");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine("=========================================");
                        try
                        {
                            var query6Result = queries.GetPostStructure(id);
                            Console.WriteLine($"Пост: \"{query6Result.Post.Title}\"\n" +
                                              $"Самый длинный коммент поста: \"{query6Result.LongestComment.Body}\"\n" +
                                              $"Самый залайканный коммент поста: \"{query6Result.MostLikedComment.Body}\"\n" +
                                              $"Количество комментов под постом где или 0 лайков или длина текста < 80: {query6Result.CommentsCountUnderBadPost}\n");
                        }
                        catch (InvalidOperationException)
                        {
                            Console.WriteLine("Данных нет :(");
                        }
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 7:
                        _isExit = true;
                        break;
                    default:
                        Console.Clear();
                        break;
                } 
            }
        }

        private static string GetTodosToShow(List<Todo> todos)
        {
            string result = "Таски: \n";
            foreach (var todo in todos)
            {
                result += $"\t\"{todo.Name}\" => {todo.Name.Length} символов\n";
            }
            result += "-----------------------------------------";
            return result;
        }
    }
}
