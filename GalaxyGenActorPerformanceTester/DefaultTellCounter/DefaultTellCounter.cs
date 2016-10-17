using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenActorPerformanceTester.DefaultTellCounter
{
    public class DefaultTell
    {
        private ActorSystem _actorSystem;

        public DefaultTell()
        {
            _actorSystem = ActorSystem.Create("DefaultTellActors");
        }

        internal void Run()
        {
            //Props props = Props.Create<ActorTop>().WithDispatcher("akka.actor.high-custom-dispatcher");
            //IActorRef _actorTop = _actorSystem.ActorOf(props, "Top");

            IActorRef _actorTop = _actorSystem.ActorOf(Props.Create<ActorTop>().WithDispatcher("akka.actor.custom-dispatcher"), "top");
            _actorTop.Tell(new TestMessage());
        }
    }

    internal class ActorTop : ReceiveActor
    {
        private Stopwatch t1;
        private IActorRef _actorChild;
        private int ticks;

        public ActorTop()
        {
            Props props = Props.Create<ActorAggregator>(new List<int>() { 1000, 50 }, Self);
            _actorChild = Context.ActorOf(props);
            ticks = 0;
            t1 = new Stopwatch();

            Receive<TestMessage>(msg => start());
            Receive<TestComplete>(msg => finish());
        }

        public void start()
        {
            // start timer
            t1.Start();
            sendMessage();
        }

        private void sendMessage()
        {
            _actorChild.Tell(new TestMessage());
        }

        public void finish()
        {
            ticks++;
            if (ticks < 100)
            {
                sendMessage();
            }
            else
            {
                // end timer
                t1.Stop();
                Console.Out.WriteLine("Default Tell Counter: " + t1.ElapsedMilliseconds.ToString() + "ms, " + (100d / ((double)t1.ElapsedMilliseconds / 1000d)).ToString("F2") + "t/s");
            }

        }
    }

    internal class ActorAggregator : ReceiveActor
    {
        ICollection<IActorRef> _childActors = new HashSet<IActorRef>();
        private int _numberOfChildren;
        private int _numberOfIncompleteChildren;
        private IActorRef _parent;

        public ActorAggregator(List<int> children, IActorRef parent)
        {
            _parent = parent;

            if (children.Count > 1)
            {
                for (int i = 0; i < children.First(); i++)
                {
                    Props props = Props.Create<ActorAggregator>(children.Skip(1).ToList(), Self).WithDispatcher("akka.actor.custom-dispatcher");
                    IActorRef _actorChild = Context.ActorOf(props);
                    _childActors.Add(_actorChild);
                }
            }
            else if (children.Count == 1)
            {
                for (int i = 0; i < children.First(); i++)
                {
                    IActorRef _actorChild = Context.ActorOf<ActorChild>();
                    _childActors.Add(_actorChild);
                }
            }
            _numberOfChildren = children.First();
            _numberOfIncompleteChildren = children.First();


            Receive<TestMessage>(msg => receive());
            Receive<TestComplete>(msg => receiveCompletedMessage(msg));
        }

        internal void receive()
        {
            foreach (IActorRef child in _childActors)
            {
                child.Tell(new TestMessage());
            }
        }

        private void receiveCompletedMessage(TestComplete msg)
        {
            _numberOfIncompleteChildren--;
            if (_numberOfIncompleteChildren <= 0)
            {
                _numberOfIncompleteChildren = _numberOfChildren;
                sendCompletedMessage();
            }
        }

        private void sendCompletedMessage()
        {
            _parent.Tell(new TestComplete());
        }
    }

    internal class ActorChild : ReceiveActor
    {
        public ActorChild()
        {
            Receive<TestMessage>(msg => receive());
        }

        internal void receive()
        {
            Sender.Tell(new TestComplete());
        }
    }
}
