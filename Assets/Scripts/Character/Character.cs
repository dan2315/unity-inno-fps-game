using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private HitPoints hp;

        private void Start()
        {
            hp.HitPointsChanged += CheckForDeath;
        }

        public void DealDamage(int health)
        {
            hp.DealDamage(health);
        }

        public void AddHealth(int health)
        {
            hp.AddHealth(health);
        }

        public void AddArmor(int armor)
        {
            hp.AddArmor(armor);
        }

        private void CheckForDeath(int armor, int health)
        {
            if (health > 0) return;
            Death();
            hp.HitPointsChanged -= CheckForDeath;
        }

        protected virtual void Death()
        {
            Debug.LogWarning("Base implementation");
        }
    }
}