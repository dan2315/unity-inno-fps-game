using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class HitPoints
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int maxArmor;
        
        private int _health;
        private int _armor;

        public int MaxHealth => maxHealth;
        public int MaxArmor => maxArmor;

        public Action<int, int> HitPointsChanged;

        public void Initialize()
        {
            _health = maxHealth;
            _armor = 0;
            HitPointsChanged?.Invoke(_armor, _health);
        }
        
        public void DealDamage(int damageAmount)
        {
            _armor -= damageAmount;
            if (_armor < 0)
            {
                _health += _armor;
                _armor -= _armor;
            }
            
            HitPointsChanged?.Invoke(_armor, _health);
        }

        public void AddHealth(int health)
        {
            _health += health;
            if (_health > maxHealth) _health = maxHealth;
            HitPointsChanged?.Invoke(_armor, _health);
        }

        public void AddArmor(int armor)
        {
            _armor += armor;
            if (_armor > maxArmor) _armor = maxArmor;
            HitPointsChanged?.Invoke(_armor, _health);
        }
    }
}