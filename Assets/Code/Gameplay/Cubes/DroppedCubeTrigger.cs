using System;
using UnityEngine;

namespace Code.Gameplay.Cubes
{
    public class DroppedCubeTrigger : MonoBehaviour
    {
        public event Action<Cube> Collision;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (HasCubeComponent(collision, out Cube cube) 
                && IsCollisionWithDroppedCube(cube))
            {
                Collision?.Invoke(cube);
            }
        }
        
        private bool HasCubeComponent(Collision collision, out Cube cube) => 
            collision.transform.TryGetComponent(out cube);

        private bool IsCollisionWithDroppedCube(Cube cube) => 
            !cube.IsPickedUp;
    }
}