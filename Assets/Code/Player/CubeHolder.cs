using System;
using System.Collections.Generic;
using Code.Gameplay.Cubes;
using Code.Gameplay.Environment;
using Code.Gameplay.Tracks;
using Code.Infrastructure.Pools;
using Code.Infrastructure.Services.Pools;
using Code.Infrastructure.Services.Vibration;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.Player
{
    public class CubeHolder : MonoBehaviour
    {
        [SerializeField] private List<PickableCube> _stack;
        [SerializeField, Min(0)] private int _stackCapacity;
        [SerializeField] private ParticleSystem _stackParticle;
        [SerializeField] private CubeCollectionText _collectTextPrefab;
        [SerializeField] private DangerousCollisionTrigger _playerCollisionTrigger;
        [SerializeField, Min(0)] private int _vibrationMilliseconds;

        public event Action<Vector3> NewPlayerPosition;

        private CameraShaker _cameraShaker;
        private TrackSpawner _trackSpawner;
        private IVibrationService _vibrationService;
        private MonoPool<CubeCollectionText> _collectTextPool;

        private void Start()
        {
            _trackSpawner = FindObjectOfType<TrackSpawner>();
            _playerCollisionTrigger.DangerousCollision += ReleaseAll;
            SubscribeInitialCubes();
        }

        private void OnDestroy()
        {
            _playerCollisionTrigger.DangerousCollision -= ReleaseAll;
            UnSubscribeAll();
        }

        [Inject]
        private void Construct(IVibrationService vibrationService, CameraShaker cameraShaker, PoolService poolService)
        {
            _vibrationService = vibrationService;
            _cameraShaker = cameraShaker;
            _collectTextPool = poolService.CreatePool(_collectTextPrefab, 3);
        }

        private void ReleaseAll()
        {
            for (int i = 0; i < _stack.Count; i++)
                RemoveCube(_stack[i]);
        }

        private void SubscribeInitialCubes()
        {
            foreach (PickableCube cube in _stack)
                SubscribeCube(cube);
        }

        private void UnSubscribeAll()
        {
            foreach (PickableCube cube in _stack)
                UnSubscribeCube(cube);
        }

        private void SubscribeCube(PickableCube cube)
        {
            cube.OtherPickupCollision += OnPickupCollision;
            cube.WallCollision += OnWallCollision;
        }
        
        private void UnSubscribeCube(PickableCube cube)
        {
            cube.OtherPickupCollision -= OnPickupCollision;
            cube.WallCollision -= OnWallCollision;
        }

        private void OnPickupCollision(PickableCube other)
        {
            if (IsStackOverflow(other))
                return;

            AddCube(other);
        }

        private bool IsStackOverflow(PickableCube cube)
        {
            bool isStackFull = _stack.Count >= _stackCapacity;
            bool isOtherCube = !CollisionIsRegistered(cube);
            
            if (isStackFull && isOtherCube)
            {
                DestroyOtherCube(cube);
                return true;
            }

            return false;
        }

        private void DestroyOtherCube(PickableCube cube)
        {
            _stackParticle.Stop();
            _stackParticle.transform.position = cube.transform.position;
            _stackParticle.Play();
            Destroy(cube.gameObject);
        }

        private void AddCube(PickableCube cube)
        {
            if (CollisionIsRegistered(cube))
                return;
            
            _stack.Add(cube);
            SubscribeCube(cube);
            MakeCubeAsChildren(cube);
            RaiseCube(cube, out Vector3 cubePosition);
            PlayStackEffect(cubePosition);
            SpawnCollectText(cubePosition);
        }

        private bool CollisionIsRegistered(PickableCube cube) => 
            _stack.Contains(cube);

        private void MakeCubeAsChildren(PickableCube cube) => 
            cube.transform.SetParent(transform);

        private void RaiseCube(PickableCube cube, out Vector3 newCubePosition)
        {
            newCubePosition = GetNewCubePosition(cube);

            cube.transform.localPosition = newCubePosition;
            
            NewPlayerPosition?.Invoke(newCubePosition + Vector3.up * cube.Height);
        }

        private Vector3 GetNewCubePosition(PickableCube cube)
        {
            PickableCube preLastCubeInStack = _stack[^2];
            Vector3 prevCubePosition = preLastCubeInStack.transform.localPosition;
            Vector3 newCubePosition = prevCubePosition + Vector3.up * cube.Height;
            
            return newCubePosition;
        }

        private void PlayStackEffect(Vector3 at)
        {
            _stackParticle.Stop();
            _stackParticle.transform.localPosition = at;
            _stackParticle.Play();
        }

        private void SpawnCollectText(Vector3 at)
        {
            Vector3 worldPosition = transform.TransformPoint(at);

            _collectTextPool.Get(worldPosition, Quaternion.identity);
        }

        private void OnWallCollision(PickableCube owner)
        {
            RemoveCube(owner);
            _cameraShaker.LightShake();
            _vibrationService.Vibrate(_vibrationMilliseconds);
        }

        private void RemoveCube(PickableCube cube)
        {
            if (!CollisionIsRegistered(cube))
                return;

            cube.IsPickable = false;
            UnSubscribeCube(cube);
            _stack.Remove(cube);
            RemoveCubeParent(cube);
            _trackSpawner.MarkToGarbageCollector(cube.gameObject);
        }

        private void RemoveCubeParent(PickableCube cube) => 
            cube.transform.SetParent(null);
    }
}