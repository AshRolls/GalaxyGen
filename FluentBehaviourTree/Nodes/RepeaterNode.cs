using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Repeats the child node for infinity or until max iterations if one is set
    /// </summary>
    public class RepeaterNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;
        private int maxIterations = -1;
        private int iterations = 0;

        /// <summary>
        /// The child node to be repeated
        /// </summary>
        private IBehaviourTreeNode childNode;

        public RepeaterNode(string name) 
        {
            this.name = name;
        }

        public RepeaterNode(string name, int maxIterations)
        {
            this.name = name;
            this.maxIterations = maxIterations;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if((maxIterations > -1 || maxIterations == 0) && iterations >= maxIterations) {
                iterations = 0;
                return BehaviourTreeStatus.Success;
            }

            childNode.Tick(time);
            iterations++;

            return BehaviourTreeStatus.Running;
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            if (this.childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to RepeaterNode!");
            }

            this.childNode = child;
        }
    }
}
