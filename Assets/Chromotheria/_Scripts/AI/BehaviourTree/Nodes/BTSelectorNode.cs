namespace VH.AI.BehaviourTree
{
    public class BTSelectorNode : BTCompositeNode
    {
        public override NodeStatus Evaluate()
        {
            foreach (var child in _children)
            {
                var status = child.Evaluate();
                if (status != NodeStatus.Failure)
                    return status;
            }
            
            return NodeStatus.Failure;
        }
    }
}