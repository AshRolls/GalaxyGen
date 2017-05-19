using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Repeats child node until it returns a failure
    /// </summary>
    public class RepeatUntilFailNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// The child node to be repeated
        /// </summary>
        private IBehaviourTreeNode childNode;

        public RepeatUntilFailNode(string name) 
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            var childStatus = childNode.Tick(time);

            if(childStatus == BehaviourTreeStatus.Failure)
                return BehaviourTreeStatus.Success;

            return BehaviourTreeStatus.Running;
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            if (this.childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to RepeatUntilFailNode!");
            }

            this.childNode = child;
        }
    }
}
