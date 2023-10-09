using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class Enemy : Character
    {
        [SerializeField] private NavMeshAgent navigation;
        [SerializeField] private Weapon weapon;

        private void Update()
        {
            Rotate();
            var isPathUnobstructed = IsPathToPlayerUnobstructed();
            
            Shoot(isPathUnobstructed);
            
            if(!isPathUnobstructed)
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

        public void SetShield()
        {
            
        }

        protected override void Death()
        {
            Destroy(gameObject);
        }
    }
}