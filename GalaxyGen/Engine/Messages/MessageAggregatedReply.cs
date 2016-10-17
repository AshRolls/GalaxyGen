using System.Collections.Generic;

namespace GalaxyGen.Engine.Messages
{
    internal class MessageAggregatedReply<T>
    {
        internal List<T> replies { get; private set; }

        public MessageAggregatedReply(List<T> replies)
        {
            this.replies = replies;
        }
    }
}