using System;
using Code.PlayerLogic;
using Code.UI.HeadUpDisplay;
using VContainer.Unity;

namespace Code.Gameplay.Systems
{
    public class CubeCollectionHandler : IInitializable, IDisposable
    {
        private readonly Player _player;
        private readonly HUD _hud;

        private int _collectedNumber;

        public CubeCollectionHandler(Player player, HUD hud)
        {
            _player = player;
            _hud = hud;
        }

        public void Initialize() => 
            _player.CubeCollected += OnPlayerCollectCube;

        public void Dispose() => 
            _player.CubeCollected -= OnPlayerCollectCube;

        private void OnPlayerCollectCube() => 
            _hud.UpdateCollectedCubesNumber(++_collectedNumber);
    }
}