using UnityEngine;

namespace VH.AI
{
    public interface IAILocomotion
    {
        void Move(Vector3 destination, float distance);
        void ResetMovement();
        void Wander();
        bool IsArrived(Vector3 point, float distance);
    }
}