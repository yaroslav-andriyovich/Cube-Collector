using Cinemachine;
using UnityEngine;

namespace Code.Core.CameraManagement
{
    public class CameraProvider : MonoBehaviour
    {
        [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; } 
    }
}