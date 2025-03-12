using System.Collections.Generic;

namespace VH.AI.BehaviourTree
{
    public abstract class BTCompositeNode : BTNode
    {
        protected List<BTNode> _children = new();

        public void AddChild(BTNode child)
        {
            _children.Add(child);
        }
    }
}