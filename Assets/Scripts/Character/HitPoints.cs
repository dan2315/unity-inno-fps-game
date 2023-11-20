using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class HitPoints
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float maxArmor;
        
        private float _health;
        private float _armor;

        public float MaxHealth => maxHealth;
        public float MaxArmor => maxArmor;

        public Action<float, float> HitPointsChanged;
        public Action DamageTaken;

        public void Initialize()
        {
            _health = maxHealth;
            _armor = 0;
            HitPointsChanged?.Invoke(_armor, _health);
        }
        
        public void DealDamage(float damageAmount)
        {
            _armor -= damageAmount;
            if (_armor < 0)
            {
                _health += _armor;
                _armor -= _armor;
            }
            
            DamageTaken?.Invoke();
            HitPointsChanged?.Invoke(_armor, _health);
        }

        public void AddHealth(float health)
        {
            _health += health;
            if (_health > maxHealth) _health = maxHealth;
            HitPointsChanged?.Invoke(_armor, _health);
        }

        public void AddArmor(float armor)
        {
            _armor += armor;
            if (_armor > maxArmor) _armor = maxArmor;
            HitPointsChanged?.Invoke(_armor, _health);
        }
    }
}