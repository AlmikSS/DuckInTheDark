namespace VH.AI.BehaviourTree
{
    public abstract class BTDecoratorNode : BTNode
    {
        protected BTNode _child;

        protected BTDecoratorNode(BTNode child)
        {
            _child = child;
        }
    }
}