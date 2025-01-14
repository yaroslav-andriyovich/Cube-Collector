using System;
using UnityEngine;

namespace Code.Gameplay.Cubes
{
    public class WallAlignmentAssistant : MonoBehaviour
    {
        private Action _alignmentCallback;
        
        public void Initialize(Action alignmentCallback) => 
            _alignmentCallback = alignmentCallback;

        public void LeanAgainstWall(Collider wall)
        {
            Transform wallTransform = wall.transform;
            Vector3 wallPosition = wallTransform.position;
            Vector3 wallSize = wallTransform.localScale;

            Transform cubeTransform = transform;
            Vector3 cubeSize = cubeTransform.localScale;
            Vector3 cubePosition = cubeTransform.position;
            
            Vector3 direction = cubePosition - wallPosition;
            
            direction.Normalize();

            bool isFrontCollision = direction.z < 0;
            bool isLeftCollision = direction.x > 0;
            bool isRightCollision = direction.x < 0;
            bool isBackCollision = direction.z > 0;

            if (isFrontCollision)
                ChangePositionNearWall(new Vector3(cubePosition.x, cubePosition.y, wallPosition.z - wallSize.z / 2f - cubeSize.z / 2f));
            else if (isLeftCollision)
                ChangePositionNearWall(new Vector3(wallPosition.x + wallSize.x / 2f + cubeSize.x / 2f, cubePosition.y, cubePosition.z));
            else if (isRightCollision)
                ChangePositionNearWall(new Vector3(wallPosition.x - wallSize.x / 2f - cubeSize.x / 2f, cubePosition.y, cubePosition.z));
            else if (isBackCollision)
                ChangePositionNearWall(new Vector3(cubePosition.x, cubePosition.y, wallPosition.z + wallSize.z / 2 + cubeSize.z / 2f));
        }

        private void ChangePositionNearWall(Vector3 at)
        {
            transform.position = at;
            _alignmentCallback?.Invoke();
        }
    }
}