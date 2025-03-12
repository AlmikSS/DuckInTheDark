namespace VH.AI.BehaviourTree
{
    public abstract class BTActionNode : BTNode
    {
        public override NodeStatus Evaluate()
        {
            return PerformAction();
        }

        protected abstract NodeStatus PerformAction();
    }
}