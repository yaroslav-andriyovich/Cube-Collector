using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI
{
    public class TouchZone : MonoBehaviour, IPointerDownHandler
    {
        public event Action Touch;

        public void OnPointerDown(PointerEventData eventData) => 
            Touch?.Invoke();
    }
}