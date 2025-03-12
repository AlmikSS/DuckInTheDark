namespace VH.AI.BehaviourTree
{
    public abstract class BTConditionNode : BTNode
    {
        public override NodeStatus Evaluate()
        {
            return CheckCondition() ? NodeStatus.Success : NodeStatus.Failure;
        }
        
        protected abstract bool CheckCondition();
    }
}