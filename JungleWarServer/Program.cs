using System;
using JungleWarServer.MessageHandle;
using JungleWarServer.Test;

namespace JungleWarServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Start");
            TestClass test = new TestClass();
            test.TestHandler();
        }
    }
}
