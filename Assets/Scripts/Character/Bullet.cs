using UnityEngine;

namespace Character
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private ParticleSystem explosionPrefab;
        
        
        private void Update()
        {
            rigidbody.velocity = transform.forward * speed;
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.transform.TryGetComponent(out Enemy enemy))
            {
                enemy.DealDamage(5);
            }

            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosion.gameObject, 1);
        }
    }
}