using Cinemachine;
using UnityEngine;

namespace Code.Gameplay.CameraManagement
{
    public class CameraProvider : MonoBehaviour
    {
        [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; } 
    }
}