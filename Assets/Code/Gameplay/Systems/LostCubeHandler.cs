using System;
using Code.Gameplay.Cubes;
using Code.Gameplay.Tracks;
using Code.PlayerLogic;
using UnityEngine;
using VContainer.Unity;

namespace Code.Gameplay.Systems
{
    public class LostCubeHandler : IInitializable, IDisposable
    {
        private const float MAX_RAY_DISTANCE = 20f;
        private const string GROUND_LAYER = "Ground";
        
        private readonly Player _player;
        private readonly int _trackGroundLayerMask = 1 << LayerMask.NameToLayer(GROUND_LAYER);

        public LostCubeHandler(Player player) => 
            _player = player;

        public void Initialize() => 
            _player.CubeLost += OnPlayerLostCube;

        public void Dispose() => 
            _player.CubeLost -= OnPlayerLostCube;

        private void OnPlayerLostCube(Cube cube)
        {
            if (TryAttachLostCubeToTrackUnderPlayer(cube))
                return;

            ReturnCubeToPool(cube);
        }

        private bool TryAttachLostCubeToTrackUnderPlayer(Cube cube)
        {
            if (IsPlayerAboveGround(GetRayFromPlayerToGround(), out RaycastHit hitInfo)
                && TryGetTrack(hitInfo, out Track track))
            {
                track.AttachCube(cube);
                return true;
            }
            
            return false;
        }

        private bool IsPlayerAboveGround(Ray ray, out RaycastHit hitInfo) => 
            UnityEngine.Physics.Raycast(ray, out hitInfo, MAX_RAY_DISTANCE, _trackGroundLayerMask);

        private Ray GetRayFromPlayerToGround() => 
            new Ray(_player.transform.position + Vector3.up, Vector3.down);

        private bool TryGetTrack(RaycastHit hitInfo, out Track track) => 
            hitInfo.collider.transform.parent.parent.TryGetComponent(out track);

        private void ReturnCubeToPool(Cube cube) => 
            cube.Release();
    }
}