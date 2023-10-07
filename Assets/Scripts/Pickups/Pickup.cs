using System;
using Character;
using UnityEngine;

namespace Pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Transform _visuals;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayableCharacter character))
            {
                ApplyPickup(character);
            }
        }

        protected virtual void ApplyPickup(PlayableCharacter character)
        {
            Debug.LogWarning("Base Implementation");
        }
    }


}