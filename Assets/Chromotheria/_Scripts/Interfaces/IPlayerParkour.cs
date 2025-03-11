using UnityEngine;

public interface IPlayerParkour
{
    bool CheckObstacle(Vector3 direction);
    void ApplyJump();
    bool CheckLedge(Vector3 direction);
    void ApplyLedge();
}