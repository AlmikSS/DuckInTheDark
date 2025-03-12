using System.Collections;
using UnityEngine;

namespace VH.AI.BehaviourTree
{
    public class BehaviourTreeController : MonoBehaviour
    {
        private BTNode _rootNode;
        private Coroutine _updateBehaviourTreeCoroutine;
        private float _updateInterval = 0.01f;

        public void Setup(BTNode rootNode, float updateInterval)
        {
            _rootNode = rootNode;
            _updateInterval = updateInterval;
        }

        public void StartBehaviourTree()
        {
            _updateBehaviourTreeCoroutine = StartCoroutine(UpdateBehaviourTreeRoutine());
        }

        public void StopBehaviourTree()
        {
            StopCoroutine(_updateBehaviourTreeCoroutine);
        }
        
        private IEnumerator UpdateBehaviourTreeRoutine()
        {
            while (_rootNode != null)
            {
                _rootNode.Evaluate();
                yield return new WaitForSecondsRealtime(_updateInterval);
            }
        }
    }
}