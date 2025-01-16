using System;
using Code.Gameplay.Tracks;

namespace Code.Gameplay.Systems
{
    public class TrackDeSpawner
    {
        public event Action<Track> DeSpawned;

        public void DeSpawn(Track track)
        {
            track.Release();
            DeSpawned?.Invoke(track);
        }
    }
}