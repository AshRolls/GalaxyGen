using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenActorPerformanceTester.DefaultTellCounter;

namespace GalaxyGenActorPerformanceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            DefaultTell dt = new DefaultTell();
            dt.Run();
            Console.ReadLine();
        }
    }

    internal class TestMessage
    {
    }

    internal class TestComplete
    {
    }

  
}
