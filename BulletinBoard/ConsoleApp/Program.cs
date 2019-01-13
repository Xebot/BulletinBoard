using System;
using BulletinDomain;
using WebApi.Contracts.DTO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            consoleUI.Start();
        }
    }
}
