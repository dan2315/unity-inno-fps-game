using System;
using Pickups;
using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private HitPoints hp;

        public HitPoints HP => hp;

        public Action OnDeath;
        protected virtual void Awake()
        {
            hp.Initialize();
            hp.HitPointsChanged += CheckForDeath;
        }

        public virtual void DealDamage(float damageAmount, ModifierType damageSourceType)
        {
            hp.DealDamage(damageAmount);
        }

        public void AddHealth(float health)
        {
            hp.AddHealth(health);
        }

        public void AddArmor(float armor)
        {
            hp.AddArmor(armor);
        }

        private void CheckForDeath(float armor, float health)
        {
            if (health > 0) return;
            Death();
            OnDeath?.Invoke();
            hp.HitPointsChanged -= CheckForDeath;
        }

        protected virtual void Death()
        {
            Debug.LogWarning("Base implementation");
        }
    }
}