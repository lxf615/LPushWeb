using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPush.Queue.Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            ReqRepBroker broker = new ReqRepBroker();
            broker.Start();
            System.Console.ReadLine();
        }
    }
}
