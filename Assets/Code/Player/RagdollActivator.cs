using System.Collections.Generic;
using Code.Physics;
using UnityEngine;

namespace Code.Player
{
    public class RagdollActivator : MonoBehaviour
    {
        [Header("Ragdoll")]
        [SerializeField] private Animator _animator;
        [SerializeField] private List<Collider> _characterColliders;
        [SerializeField] private List<Rigidbody> _characterRigidbodyGroup;

        [Header("Movement")]
        [SerializeField] private Rigidbody _rigidbodyForMovement;
        [SerializeField] private BoxCollider _colliderForMovement;
        [SerializeField] private GravityBooster _attractionForMovement;

        public void Activate()
        {
            DisableComponentsForMovement();
            EnableColliders();
            EnablePhysics();
        }

        private void DisableComponentsForMovement()
        {
            _rigidbodyForMovement.isKinematic = true;
            _colliderForMovement.enabled = false;
            _attractionForMovement.enabled = false;
            _animator.enabled = false;
        }

        private void EnableColliders()
        {
            foreach (Collider characterCollider in _characterColliders)
                characterCollider.enabled = true;
        }

        private void EnablePhysics()
        {
            foreach (Rigidbody characterRigidbody in _characterRigidbodyGroup)
                characterRigidbody.isKinematic = false;
        }
    }
}