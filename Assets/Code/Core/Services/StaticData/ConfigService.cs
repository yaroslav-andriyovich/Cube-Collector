using Code.Core.AssetManagement;
using Code.StaticData;

namespace Code.Core.Services.StaticData
{
    public class ConfigService
    {
        private readonly IAssetProvider _assetProvider;
        
        private CameraShakeConfig _cameraShakeShakeConfig;
        private TrackSpawningConfig _trackSpawningConfig;

        public ConfigService(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public void LoadAll()
        {
            _cameraShakeShakeConfig = _assetProvider.Load<CameraShakeConfig>(AssetsPath.StaticData + "/Camera/CameraShakeConfig");
            _trackSpawningConfig = _assetProvider.Load<TrackSpawningConfig>(AssetsPath.StaticData + "/Tracks/TrackSpawningConfig");
        }

        public CameraShakeConfig GetCameraShake() => 
            _cameraShakeShakeConfig;
        
        public TrackSpawningConfig GetTrackSpawner() => 
            _trackSpawningConfig;
    }
}