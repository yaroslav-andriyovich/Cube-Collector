using System;
using Code.Core.Pools.Poolable;
using UnityEngine;

namespace Code.Gameplay.Cubes
{
    public class Cube : PoolableBase
    {
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private DroppedCubeTrigger _droppedCubeTrigger;
        [SerializeField] private WallTrigger _wallTrigger;
        [SerializeField] private WallAlignmentAssistant _wallAlignmentAssistant;

        public event Action<Cube> WallCollision;
        public event Action<Cube> CollisionWithDropped;
        public event Action<Cube> PickedUp;
        
        public float Height => _boxCollider.size.y;
        public bool IsPickedUp { get; private set; }

        private void Awake()
        {
            _wallAlignmentAssistant.Initialize(NotifyWallCollision);
            
            _droppedCubeTrigger.Collision += OnDroppedCubeCollision;
            _wallTrigger.Collision += _wallAlignmentAssistant.LeanAgainstWall;
        }

        private void OnEnable() => 
            IsPickedUp = false;

        private void OnDestroy()
        {
            _droppedCubeTrigger.Collision -= OnDroppedCubeCollision;
            _wallTrigger.Collision -= _wallAlignmentAssistant.LeanAgainstWall;
        }

        public void PickUp()
        {
            if (IsPickedUp)
                return;

            IsPickedUp = true;
            PickedUp?.Invoke(this);
        }

        private void OnDroppedCubeCollision(Cube cube)
        {
            if (IsPickedUp)
                CollisionWithDropped?.Invoke(cube);
        }
        
        private void NotifyWallCollision() => 
            WallCollision?.Invoke(this);
    }
}