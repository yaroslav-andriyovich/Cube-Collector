using Code.Core.AssetManagement;
using Code.StaticData;

namespace Code.Core.Services.StaticData
{
    public class StaticDataService
    {
        private readonly IAssetProvider _assetProvider;
        
        private CameraConfig _cameraConfig;
        private TrackSpawningConfig _trackSpawningConfig;

        public StaticDataService(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public void LoadAll()
        {
            _cameraConfig = _assetProvider.Load<CameraConfig>(AssetsPath.StaticData + "/Camera/CameraConfig");
            _trackSpawningConfig = _assetProvider.Load<TrackSpawningConfig>(AssetsPath.StaticData + "/Tracks/TrackSpawningConfig");
        }

        public CameraConfig ForCamera() => 
            _cameraConfig;
        
        public TrackSpawningConfig ForTrackSpawner() => 
            _trackSpawningConfig;
    }
}