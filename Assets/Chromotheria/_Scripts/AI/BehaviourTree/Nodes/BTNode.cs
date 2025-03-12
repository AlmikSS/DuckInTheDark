namespace VH.AI.BehaviourTree
{
    public abstract class BTNode
    {
        public abstract NodeStatus Evaluate();
    }
    
    public enum NodeStatus
    {
        Failure,
        Success,
        Running,
    }
}