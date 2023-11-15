using Pickups;
using UnityEngine;

namespace Character
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private ParticleSystem explosionPrefab;

        private ModifierType _modifier = ModifierType.None;
        
        private void Update()
        {
            rigidbody.velocity = transform.forward * speed;
        }

        public void SetDamageModifier(ModifierType modifier)
        {
            _modifier = modifier;
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.transform.TryGetComponent(out Character enemy))
            {
                enemy.DealDamage(5, _modifier);
            }

            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosion.gameObject, 1);
        }
    }
}