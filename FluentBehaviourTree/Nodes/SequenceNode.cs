using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        IEnumerator<IBehaviourTreeNode> enumerator;

        public SequenceNode(string name)
        {
            this.name = name;
        }

        public void Init()
        {
            enumerator = children.GetEnumerator();
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (enumerator == null)
                Init();

            if (enumerator.Current == null)
                enumerator.MoveNext();

            while(enumerator.Current != null)
            {
                var childStatus = enumerator.Current.Tick(time);

                if (childStatus != BehaviourTreeStatus.Success)
                {
                    if (childStatus == BehaviourTreeStatus.Failure)
                        enumerator.Reset();
                    return childStatus;
                }               

                if (!enumerator.MoveNext())
                    break;
            }

            enumerator.Reset();
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Add a child to the sequence.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
