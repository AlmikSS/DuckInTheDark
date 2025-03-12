using UnityEngine;

namespace VH.AI
{
    public abstract class AIBrain : MonoBehaviour
    {
        private GameObject _target;
    
        public GameObject Target => _target;

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        public void ResetTarget()
        {
            _target = null;
        }
    }
}