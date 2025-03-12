using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VH.AI
{
    public class AIVision : MonoBehaviour
    {
        [SerializeField] private Transform _visionOrigin;
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private float _visionRange;
        [SerializeField] private float _viewAngle;
        [SerializeField] private float _visionUpdateDelay;
        [SerializeField] private bool _drawGizmos;

        private List<GameObject> _visibleObjects = new();

        public IReadOnlyList<GameObject> VisibleObjects => _visibleObjects;

        private void Start()
        {
            StartCoroutine(VisionRoutine());
        }

        private IEnumerator VisionRoutine()
        {
            while (true)
            {
                _visibleObjects.Clear();
                ApplyVision();
                yield return new WaitForSeconds(_visionUpdateDelay);
            }
        }

        private void ApplyVision()
        {
            var colliders = Physics.OverlapSphere(_visionOrigin.position, _visionRange, _targetLayerMask);

            foreach (var collider in colliders)
            {
                if (CanSee(collider))
                {
                    _visibleObjects.Add(collider.gameObject);
                }
            }
        }

        private bool CanSee(Collider target)
        {
            var bounds = target.bounds;
            var min = bounds.min;
            var max = bounds.max;
            var points = new Vector3[8]
            {
                new(min.x, min.y, min.z),
                new(min.x, min.y, max.z),
                new(min.x, max.y, min.z),
                new(min.x, max.y, max.z),
                new(max.x, min.y, min.z),
                new(max.x, min.y, max.z),
                new(max.x, max.y, min.z),
                new(max.x, max.y, max.z)
            };

            foreach (var point in points)
            {
                var directionToTarget = (point - _visionOrigin.position).normalized;

                if (Vector3.Angle(_visionOrigin.forward, directionToTarget) < _viewAngle / 2)
                {
                    var distanceToTarget = Vector3.Distance(_visionOrigin.position, point);
                    if (!Physics.Raycast(_visionOrigin.position, directionToTarget, distanceToTarget, _obstacleLayerMask))
                    {
                        if (_drawGizmos)
                            Debug.DrawLine(_visionOrigin.position, point, Color.green);
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_visionOrigin.position, _visionRange);
        }
    }
}