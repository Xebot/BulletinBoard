using System;
using System.Collections.Generic;
using System.Text;
using BulletinDomain;
using WebApi.Contracts.DTO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace ConsoleApp
{
    public class ConsoleUI
    {
        public async void Start()
        {
            Console.WriteLine("Список всех объявлений:");
            Console.WriteLine();
            
            IList<AdvertDto> ads = GetFromAdvertService().Result;
            PrintResult(ads);

            //Редактирование одного объявления
            //Console.WriteLine("Введите ID объявления которое хотите отредактировать:");
            //int redID = Convert.ToInt32(Console.ReadLine());
            //Console.Clear();
            //AdvertDto ad = GetAdvertFromAdvertService(redID).Result;
            //PrintOneResult(ad);
            //EditAdvert(ad);

            //Добавление объявления
            CreateAdvert();
            //Удаление объявления
            //DeleteAdvert();
            
            Console.ReadLine();
            
        }
        private async void DeleteAdvert()
        {
            Console.WriteLine("Введите id объявления которое надо удалить");
            int id = Convert.ToInt32(Console.ReadLine());
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:58886/api/adverts/{id}");
                
                
            }
        }
        private async void CreateAdvert()
        {
            AdvertDto newadvert = new AdvertDto();
            Console.WriteLine("Введите текст новго объявления");
            newadvert.AdvertText = Console.ReadLine();
            Console.WriteLine("Введите новую цену");
            newadvert.Price = Convert.ToDecimal(Console.ReadLine());
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<AdvertDto>("http://localhost:58886/api/adverts/add", newadvert);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Объявление добавлено");
                    Console.ReadLine();
                }
            }
        }
        private async void EditAdvert(AdvertDto ad)
        {
            Console.WriteLine("Введите новый текст объявления");
            ad.AdvertText = Console.ReadLine();
            Console.WriteLine("Введите новую цену");
            ad.Price = Convert.ToDecimal(Console.ReadLine());
            using (var httpClient = new HttpClient())
            {
                
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<AdvertDto>("http://localhost:58886/api/adverts/save", ad);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("все обновлено");
                    Console.ReadLine();
                }
            }

        }
        private void PrintOneResult(AdvertDto ad)
        {
            if (ad == null)
            {
                Console.WriteLine("----ПУСТО----");
            }
            else
            {
                
                Console.WriteLine("----Объявление:----");
                Console.WriteLine($"Id:{ad.Id}, Text: {ad.AdvertText}, Price: {ad.Price}");
                
                if (ad.Comments!= null && ad.Comments.Count != 0)
                {
                    Console.WriteLine("----Комментарии:----");
                    foreach (var comment in ad.Comments)
                    {
                        Console.WriteLine($"Id:{comment.Id}, Text: {comment.CommentText}");
                    }
                    Console.WriteLine("----Конец комментариев----");
                }
                    
                
            }
            Console.WriteLine("----END----");
        }
        private void PrintResult (IList<AdvertDto> ads)
        {
            if (ads == null)
            {
                Console.WriteLine("----ПУСТО----");
            }
            else
            {
                Console.WriteLine("Adverts:");
                foreach (var ad in ads)
                {
                    PrintOneResult(ad);
                }
            }
            Console.WriteLine("----END----");
        }
        private async Task<IList<AdvertDto>> GetFromAdvertService()
        {
            IList<AdvertDto> result = null;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync("http://localhost:58886/api/adverts/All").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<IList<AdvertDto>>();
                }
            }
            return result;
        }
        private async Task<AdvertDto> GetAdvertFromAdvertService(int id)
        {
            AdvertDto result = null;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync($"http://localhost:58886/api/adverts/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<AdvertDto>();
                }
            }
            return result;

        }
    }
}
