using System.Collections;
using UnityEngine;

public class PlayerParkour : MonoBehaviour, IPlayerParkour
{
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _climbHeight;
    [SerializeField] private float _obstacleCheckDistance;
    [SerializeField] private float _climbSpeed;
    [SerializeField] private float _climbAngle;
    
    public bool CheckObstacle(Vector3 direction, Vector3 origin, out Vector3 point)
    {
        if (Physics.Raycast(origin, direction, out var hit, _obstacleCheckDistance, _obstacleLayerMask))
        {
            var center = new Vector3(hit.point.x, hit.collider.bounds.max.y, hit.point.z);
            point = center;
            var maxY = center.y;
            var y = transform.position.y;
            if (maxY - y > _climbHeight)
                return false;

            var angle = Mathf.Abs(Vector3.Angle(Vector3.up, hit.normal));
            if (angle <= _climbAngle)
                return false;
                    
            if (!Physics.Raycast(center, Vector3.up, _playerHeight + 0.1f, _obstacleLayerMask))
                return true;
        }

        point = default;
        return false;
    }

    public IEnumerator ApplyJump(Vector3 point)
    {
        var standPoint = new Vector3(point.x, point.y + _playerHeight / 2, point.z);
        
        while (Vector3.Distance(transform.position, standPoint) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, standPoint, _climbSpeed * Time.deltaTime);
            yield return null;
        }
    }
}