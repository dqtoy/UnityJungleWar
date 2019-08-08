using System;
using JungleWarServer.MessageHandle;
using JungleWarServer.Handler;
using JungleWarServer.NetFrame;

namespace JungleWarServer.Test
{
    public class TestClass
    {
        public void TestByteArray()
        {
            ByteArray array = new ByteArray(3);
            array.AddLast(new byte[2] { 9, 9 });
            array.TestPrint();
            array.AddLast(new byte[5] { 1, 2, 3, 4, 5 });
            array.TestPrint();
            byte[] head = array.PopRange(4);
            array.TestPrint();
            foreach(byte item in head)
            {
                Console.Write(item + " ");
            }
            array.AddLast(new byte[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            byte[] peek = array.PeekRange(5);
            array.TestPrint();
            foreach(byte item in peek)
                Console.Write(item + " ");
        }

        public void TestHandler()
        {
            HandlerCenter center = new HandlerCenter();
            Server server = new Server("127.0.0.1", 8899);
            server.RegisterAccpetEvent(center.OnClientAccept);
            server.RegisterCloseEvent(center.OnClientClose);
            server.RegisterReceiveEvent(center.OnClientReceive);
            server.Start();

            Console.ReadLine();
        }
    }
}