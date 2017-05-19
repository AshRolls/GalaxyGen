using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Decorator node that always succeeds.
    /// </summary>
    public class SucceederNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// The child to be run no matter what.
        /// </summary>
        private IBehaviourTreeNode childNode;

        public SucceederNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (childNode == null)
            {
                throw new ApplicationException("Succeeder must have a child node!");
            }

            childNode.Tick(time);
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            if (this.childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to SucceederNode!");
            }

            this.childNode = child;
        }
    }
}
