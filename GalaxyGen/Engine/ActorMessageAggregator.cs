using Akka.Actor;
using GalaxyGen.Engine.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    class ActorMessageAggregator<T> : ReceiveActor
    {
        private IActorRef originalSender;
        private ISet<IActorRef> refs;

        public ActorMessageAggregator(ISet<IActorRef> refs)
        {
            this.refs = refs;
            // this operation will finish after 30 sec of inactivity
            // (when no new message arrived)
            Context.SetReceiveTimeout(TimeSpan.FromSeconds(30));
            ReceiveAny(x =>
            {
                originalSender = Sender;
                foreach (var aref in refs) aref.Tell(x);
                Become(Aggregating);
            });
        }

        private void Aggregating()
        {
            var replies = new List<T>();
            // when timeout occurred, we reply with what we've got so far
            Receive<ReceiveTimeout>(_ => ReplyAndStop(replies));
            Receive<T>(x =>
            {
                if (refs.Remove(Sender)) replies.Add(x);
                if (refs.Count == 0) ReplyAndStop(replies);
            });
        }

        private void ReplyAndStop(List<T> replies)
        {
            originalSender.Tell(new MessageAggregatedReply<T>(replies));
            Context.Stop(Self);
        }
    }
}
