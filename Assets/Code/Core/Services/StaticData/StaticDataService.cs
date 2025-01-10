using Code.StaticData;
using UnityEngine;

namespace Code.Core.Services.StaticData
{
    public class StaticDataService
    {
        private CameraConfig _cameraConfig;
        private TrackSpawningConfig _trackSpawningConfig;

        public void LoadAll()
        {
            _cameraConfig = Resources.Load<CameraConfig>(AssetsPath.StaticData + "/CameraConfig");
            _trackSpawningConfig = Resources.Load<TrackSpawningConfig>(AssetsPath.StaticData + "/TrackSpawningConfig");
        }

        public CameraConfig ForCamera() => 
            _cameraConfig;
        
        public TrackSpawningConfig ForTrackSpawner() => 
            _trackSpawningConfig;
    }
}