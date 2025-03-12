using System;

namespace VH.AI.BehaviourTree
{
    public class BTInverseDecoratorNode : BTDecoratorNode
    {
        public BTInverseDecoratorNode(BTNode child) : base(child)
        {
        }

        public override NodeStatus Evaluate()
        {
            var status = _child.Evaluate();

            switch (status)
            {
                case NodeStatus.Failure:
                    return NodeStatus.Success;
                case NodeStatus.Success:
                    return NodeStatus.Failure;
            }
            
            return NodeStatus.Running;
        }
    }
}