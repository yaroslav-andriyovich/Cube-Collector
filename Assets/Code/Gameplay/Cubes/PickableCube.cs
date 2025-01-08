using System;
using Code.Core.Pools.Poolable;
using UnityEngine;

namespace Code.Gameplay.Cubes
{
    public class PickableCube : PoolableBase<PickableCube>
    {
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private PickableWallTrigger _wallTrigger;

        public event Action<PickableCube> WallCollision;
        public event Action<PickableCollisionData> CollisionWithDropped;
        public event Action<PickableCube> PickedUp;
        public float Height => _boxCollider.size.y;
        public bool IsPickedUp { get; private set; }

        private void Start() => 
            _wallTrigger.WallCollision += CheckNearWall;

        private void OnEnable() => 
            IsPickedUp = false;

        private void OnDestroy() => 
            _wallTrigger.WallCollision -= CheckNearWall;

        public void PickUp()
        {
            if (IsPickedUp)
                return;
            
            PickedUp?.Invoke(this);
            IsPickedUp = true;
        }

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
            if (HasPickableComponent(collision, out PickableCube cube) 
                && IsPickedUp && !cube.IsPickedUp)
            {
                PickableCollisionData collisionData = new PickableCollisionData
                {
                    owner = this,
                    other = cube
                };

                CollisionWithDropped?.Invoke(collisionData);
            }
        }

        private void ChangePositionNearWall(Vector3 at)
        {
            transform.position = at;
            
            WallCollision?.Invoke(this);
        }

        private bool HasPickableComponent(Collision collision, out PickableCube pickableCube) => 
            collision.transform.TryGetComponent(out pickableCube);
    }

    public class PickableCollisionData
    {
        public PickableCube owner;
        public PickableCube other;
    }
}