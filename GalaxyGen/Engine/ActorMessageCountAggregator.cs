using Akka.Actor;
using GalaxyGen.Engine.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorMessageCountAggregator<T>: ReceiveActor
    {
        private IActorRef _replyActor;
        private int refCount;

        public ActorMessageCountAggregator(ISet<IActorRef> refs, IActorRef replyActor)
        {
            this.refCount = refs.Count;
            _replyActor = replyActor;
            //Context.SetReceiveTimeout(TimeSpan.FromMilliseconds(timeoutMillisecs));
            ReceiveAny(msg =>
            {                
                foreach (var aref in refs)
                {
                    aref.Tell(msg);
                }
                Become(Counting);
            });
        }

        private void Counting()
        {
            // when timeout occurred, we reply with what we've got so far
            Receive<ReceiveTimeout>(_ => ReplyAndStop());
            Receive<T>(x =>
            {
                refCount--;
                if (refCount <= 0) ReplyAndStop();
            });
        }

        private void ReplyAndStop()
        {
            _replyActor.Tell(new MessageCountCompletedReply());
            Context.Stop(Self);
        }
    }
}
