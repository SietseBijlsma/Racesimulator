using System;
using System.Threading;
using Controller;
using Model;

namespace Racebaan
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRace();
            
            Console.WriteLine(Data.CurrentRace);

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
