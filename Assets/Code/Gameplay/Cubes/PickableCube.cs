using System;
using UnityEngine;

namespace Code.Gameplay.Cubes
{
    public class PickableCube : MonoBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private PickableWallTrigger _wallTrigger;

        public event Action<PickableCube> WallCollision;
        public event Action<PickableCube> OtherPickupCollision;
        public float Height => _boxCollider.size.y;
        public bool IsPickable { get; set; }
        
        private const string PickupTag = "Pickable";

        private void Start()
        {
            _wallTrigger.WallCollision += CheckNearWall;
            
            IsPickable = true;
        }

        private void OnDestroy() => 
            _wallTrigger.WallCollision -= CheckNearWall;

        private void CheckNearWall(Collider wall)
        {
            Transform wallTransform = wall.transform;
            Vector3 wallPosition = wallTransform.position;
            Vector3 wallSize = wallTransform.localScale;

            Transform currentTransform = transform;
            Vector3 currentSize = currentTransform.localScale;
            Vector3 currentPosition = currentTransform.position;
            
            Vector3 direction = currentPosition - wallPosition;
            
            direction.Normalize();

            bool isFrontCollision = direction.z < 0;
            bool isLeftCollision = direction.x > 0;
            bool isRightCollision = direction.x < 0;
            bool isBackCollision = direction.z > 0;

            if (isFrontCollision)
                ChangePositionNearWall(new Vector3(currentPosition.x, currentPosition.y, wallPosition.z - wallSize.z / 2f - currentSize.z / 2f));
            else if (isLeftCollision)
                ChangePositionNearWall(new Vector3(wallPosition.x + wallSize.x / 2f + currentSize.x / 2f, currentPosition.y, currentPosition.z));
            else if (isRightCollision)
                ChangePositionNearWall(new Vector3(wallPosition.x - wallSize.x / 2f - currentSize.x / 2f, currentPosition.y, currentPosition.z));
            else if (isBackCollision)
                ChangePositionNearWall(new Vector3(currentPosition.x, currentPosition.y, wallPosition.z + wallSize.z / 2 + currentSize.z / 2f));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsPickable)
                return;

            if (HasPickupTag(collision) 
                && HasPickupComponent(collision, out PickableCube cube) 
                && cube.IsPickable)
            {
                OtherPickupCollision?.Invoke(cube);
            }
        }

        private void ChangePositionNearWall(Vector3 at)
        {
            transform.position = at;
            
            WallCollision?.Invoke(this);
        }

        private bool HasPickupTag(Collision collision) => 
            collision.gameObject.CompareTag(PickupTag);

        private bool HasPickupComponent(Collision collision, out PickableCube pickableCube) => 
            collision.transform.TryGetComponent<PickableCube>(out pickableCube);
    }
}