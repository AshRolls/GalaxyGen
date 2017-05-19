using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: optimization, bake this to an array.

        IEnumerator<IBehaviourTreeNode> enumerator;

        public SelectorNode(string name)
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
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    if (childStatus == BehaviourTreeStatus.Success)
                        enumerator.Reset();
                    return childStatus;
                }

                if (!enumerator.MoveNext())
                    break;
            }

            enumerator.Reset();
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Add a child node to the selector.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
