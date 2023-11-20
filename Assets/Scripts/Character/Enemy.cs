using Pickups;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class Enemy : Character
    {
        [SerializeField] private NavMeshAgent navigation;
        [SerializeField] private Weapon weapon;

        [SerializeField] private SphereCollider forceshieldCollider;
        [SerializeField] private ShieldVisuals shieldVisuals;

        [SerializeField] private Animator animator;
        

        private bool _deadState;
        
        private ModifierType _shieldType = ModifierType.None;
        private static readonly int DeathAnimation = Animator.StringToHash("Death");

        private void Update()
        {
            if (_deadState) return;
            
            Rotate();
            var isPathUnobstructed = IsPathToPlayerUnobstructed();
            var distanceToPlayer = Vector3.Distance(PlayableCharacter.Instance.transform.position, transform.position);

            Shoot(isPathUnobstructed);
            
            if(!isPathUnobstructed || distanceToPlayer > 50)
            {
                MoveToPlayer();
            } 
        }

        private void MoveToPlayer()
        {
            if (navigation.isStopped)
            {
                navigation.isStopped = false;
                navigation.SetDestination(PlayableCharacter.Instance.transform.position);
            }
        }

        private void Shoot(bool shootingCondition)
        {
            if (!navigation.isStopped) navigation.isStopped = true;
            
            weapon.Shoot(shootingCondition, PlayableCharacter.Instance.transform.position + 1.5f*Vector3.up);
        }

        private bool IsPathToPlayerUnobstructed()
        {
            var direction = PlayableCharacter.Instance.transform.position - transform.position;
            var rayToPlayer = new Ray(transform.position + 1.5f*Vector3.up, direction);
            Physics.Raycast(rayToPlayer, out var hit);
            return hit.transform.CompareTag("Player");
        }
        
        private void Rotate()
        {
            var direction = PlayableCharacter.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        public void SetShield(ModifierType modifier)
        {
            _shieldType = modifier;
            shieldVisuals.gameObject.SetActive(true);
            shieldVisuals.SetMaterial(modifier);
            forceshieldCollider.gameObject.SetActive(true);
            HP.HitPointsChanged += (armor, _) =>
            {
                if (armor <= 0) DestroyShield();
            };
        }

        private void DestroyShield()
        {
            shieldVisuals.gameObject.SetActive(false);
            forceshieldCollider.gameObject.SetActive(false);
            _shieldType = ModifierType.None;
        }

        public override void DealDamage(float damageAmount, ModifierType damageSourceType)
        {
            if (_shieldType != ModifierType.None)
            {
                if (_shieldType != damageSourceType)
                {
                    damageAmount *= 2f;
                }
                else
                {
                    damageAmount *= 0.25f;
                }
            }
            else if (damageSourceType != ModifierType.None)
            {
                damageAmount *= 2f;
            }
            
            HP.DealDamage(damageAmount);
        }

        protected override void Death()
        {
            Destroy(GetComponent<Rigidbody>());
            animator.SetTrigger(DeathAnimation);
            _deadState = true;
            Destroy(gameObject, 5);
        }
    }
}