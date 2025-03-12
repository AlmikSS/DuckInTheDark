using System.Collections;
using UnityEngine;

public interface IPlayerParkour
{
    bool CheckObstacle(Vector3 direction, Vector3 origin, out Vector3 point);
    IEnumerator ApplyJump(Vector3 point);
}