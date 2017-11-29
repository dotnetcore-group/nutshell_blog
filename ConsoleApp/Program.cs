using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10000; i++)
            {
                new TaskFactory().StartNew(async () =>
                {
                    StreamReader reader = new StreamReader(@"D:\Desktop\hello.txt");
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " : " + await reader.ReadToEndAsync());
                });
            }
            Console.ReadKey();
        }
    }
}
