using Code.Core.AssetManagement;
using Code.StaticData;
using UnityEngine;

namespace Code.Core.Services.StaticData
{
    public class ConfigService
    {
        private CameraShakeConfig _cameraShakeShakeConfig;
        private TrackSpawningConfig _trackSpawningConfig;

        public void LoadAll()
        {
            _cameraShakeShakeConfig = Resources.Load<CameraShakeConfig>(AssetsPath.ConfigsCatalog + "/Camera/CameraShakeConfig");
            _trackSpawningConfig = Resources.Load<TrackSpawningConfig>(AssetsPath.ConfigsCatalog + "/Tracks/TrackSpawningConfig");
        }

        public CameraShakeConfig GetCameraShake() => 
            _cameraShakeShakeConfig;
        
        public TrackSpawningConfig GetForTracks() => 
            _trackSpawningConfig;
    }
}