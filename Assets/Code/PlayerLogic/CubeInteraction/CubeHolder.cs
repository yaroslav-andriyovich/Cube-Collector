using System;
using System.Collections.Generic;
using Code.Gameplay.Cubes;
using UnityEngine;

namespace Code.PlayerLogic.CubeInteraction
{
    public class CubeHolder : MonoBehaviour
    {
        [SerializeField] private List<Cube> _stack;
        [SerializeField, Min(0)] private int _stackCapacity;

        public event Action<Vector3> NewPlayerPosition;
        public event Action Emptied;
        public event Action<Cube> CubeCollidedWithWall;
        public event Action<Cube> StackOverflow;

        private void Start() => 
            SubscribeInitialCubes();

        private void OnDestroy() => 
            UnSubscribeAll();

        public void ReleaseAll() => 
            UnSubscribeAll();

        private void SubscribeInitialCubes()
        {
            foreach (Cube cube in _stack)
            {
                SubscribeCube(cube);
                cube.PickUp();
            }
        }

        private void UnSubscribeAll()
        {
            foreach (Cube cube in _stack)
                UnSubscribeCube(cube);
        }

        private void SubscribeCube(Cube cube)
        {
            cube.CollisionWithDropped += OnCollisionWithDropped;
            cube.WallCollision += OnWallCollision;
        }
        
        private void UnSubscribeCube(Cube cube)
        {
            cube.CollisionWithDropped -= OnCollisionWithDropped;
            cube.WallCollision -= OnWallCollision;
        }

        private void OnCollisionWithDropped(Cube other)
        {
            if (CollisionIsRegistered(other))
                return;

            if (IsStackOverflow())
            {
                DestroyDroppedCube(other);
                return;
            }

            AddCube(other);
        }

        private bool IsStackOverflow() => 
            _stack.Count >= _stackCapacity;

        private void DestroyDroppedCube(Cube cube)
        {
            cube.PickUp();
            cube.Release();
            
            StackOverflow?.Invoke(cube);
        }

        private void AddCube(Cube cube)
        {
            if (CollisionIsRegistered(cube))
                return;

            cube.PickUp();
            _stack.Add(cube);
            SubscribeCube(cube);
            MakeCubeAsChildren(cube);
            RaiseCube(cube);
        }

        private bool CollisionIsRegistered(Cube cube) => 
            _stack.Contains(cube);

        private void MakeCubeAsChildren(Cube cube) => 
            cube.transform.SetParent(transform);

        private void RaiseCube(Cube cube)
        {
            Vector3 newCubePosition = GetNewCubePosition(cube);
            
            cube.transform.localPosition = newCubePosition;
            
            NewPlayerPosition?.Invoke(newCubePosition + Vector3.up * cube.Height);
        }

        private Vector3 GetNewCubePosition(Cube cube)
        {
            Cube preLastCubeInStack = _stack[^2];
            Vector3 prevCubePosition = preLastCubeInStack.transform.localPosition;
            Vector3 newCubePosition = prevCubePosition + Vector3.up * cube.Height;
            
            return newCubePosition;
        }

        private void OnWallCollision(Cube owner)
        {
            RemoveCube(owner);
            CubeCollidedWithWall?.Invoke(owner);
        }

        private void RemoveCube(Cube cube)
        {
            if (cube == null || !CollisionIsRegistered(cube))
                return;

            UnSubscribeCube(cube);
            _stack.Remove(cube);
            cube.transform.SetParent(null);
            CheckStackEmptying();
        }

        private void CheckStackEmptying()
        {
            if (_stack.Count > 0)
                return;
            
            Emptied?.Invoke();
        }
    }
}