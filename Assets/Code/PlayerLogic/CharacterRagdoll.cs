using System.Collections.Generic;
using Code.Gameplay.Physics;
using UnityEngine;

namespace Code.PlayerLogic
{
    public class CharacterRagdoll : MonoBehaviour
    {
        [Header("Ragdoll")]
        [SerializeField] private Animator _animator;
        [SerializeField] private List<Collider> _characterColliders;
        [SerializeField] private List<Rigidbody> _characterRigidbodyGroup;

        [Header("Movement")]
        [SerializeField] private BoxCollider _colliderForMovement;
        [SerializeField] private GravityBooster _gravityBooster;

        public void Enable()
        {
            DisableComponentsForMovement();
            EnableColliders();
            EnablePhysics();
        }

        private void DisableComponentsForMovement()
        {
            _colliderForMovement.enabled = false;
            _gravityBooster.enabled = false;
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