namespace VH.AI.BehaviourTree
{
    public class BTSequenceNode : BTCompositeNode
    {
        private int _currentChild = 0;
        
        public override NodeStatus Evaluate()
        {
            while (_currentChild < _children.Count)
            {
                var status = _children[_currentChild].Evaluate();

                switch (status)
                {
                    case NodeStatus.Failure:
                        _currentChild = 0;
                        return NodeStatus.Failure;
                    case NodeStatus.Running:
                        return NodeStatus.Running;
                    case NodeStatus.Success:
                        _currentChild++;
                        break;
                }
            }

            _currentChild = 0;
            return NodeStatus.Success;
        }
    }
}