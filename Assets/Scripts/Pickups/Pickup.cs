using System;
using Character;
using DG.Tweening;
using UnityEngine;

namespace Pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Transform visuals;

        private Tween _rotation;
        private Tween _floating;

        public Action OnCollected;
        
        private void Start()
        {
            _rotation = visuals.DORotate(180 * Vector3.up, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
            _floating = visuals.DOMoveY(visuals.position.y + 0.25f, 1).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out PlayableCharacter character))
            {
                ApplyPickup(character);
                character.PlayPickupSound();
                _rotation.Kill();
                _floating.Kill();
                OnCollected?.Invoke();
                Destroy(gameObject);
            }
        }

        protected virtual void ApplyPickup(PlayableCharacter character)
        {
            Debug.LogWarning("Base Implementation");
        }


     
        
    }


}