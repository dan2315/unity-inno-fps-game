using System;
using System.Linq;
using UnityEngine;

namespace Character
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private float attackRadius;

        [SerializeField] private Transform rotationPointY;
        [SerializeField] private Transform rotationPointX;

        [SerializeField] private LayerMask attackLayer;
        


        private Enemy _enemyToAttack;
        private Collider[] _foundEnemies = new Collider[16];

        private void Update()
        {
            ScanAreaForEnemies();

            if (!_enemyToAttack) return;

            Rotate(_enemyToAttack.transform);
            Shoot(_enemyToAttack, _enemyToAttack);
        }


        private void ScanAreaForEnemies()
        {
            if (_enemyToAttack) return;

            for (var i = 0; i < _foundEnemies.Length; i++)
            {
                _foundEnemies[i] = null;
            }
                
            Physics.OverlapSphereNonAlloc(transform.position, attackRadius, _foundEnemies, attackLayer);

            var enemiesThatCanBeSeen = _foundEnemies.Where(enemy => enemy !=null && IsPathToEnemyUnobstructed(enemy.transform));
            var sortedByDistance =
                enemiesThatCanBeSeen.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position));
            _enemyToAttack = sortedByDistance.FirstOrDefault()?.transform.parent.GetComponent<Enemy>();
        }

        private void Shoot(bool shootingCondition, Enemy enemy)
        {
            if (!IsPathToEnemyUnobstructed(enemy.transform))
            {
                _enemyToAttack = null;
            }
            else weapon.Shoot(shootingCondition, enemy.transform.position + 1.5f * Vector3.up);
        }

        private bool IsPathToEnemyUnobstructed(Transform enemy)
        {
            var direction = enemy.position - transform.position;
            var rayToEnemy = new Ray(transform.position + Vector3.up, direction);
            Physics.Raycast(rayToEnemy, out var hit);
            return hit.transform.CompareTag("Enemy");
        }

        private void Rotate(Transform enemy)
        {
            var direction = enemy.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

            //rotationPointY.transform.localEulerAngles = Vector3.Lerp(rotationPointY.transform.localEulerAngles, Vector3.up * lookRotation.eulerAngles.y, 0.5f);
            //rotationPointX.transform.localEulerAngles = Vector3.Lerp(rotationPointX.transform.localEulerAngles,Vector3.forward * lookRotation.eulerAngles.x,0.5f);
            rotationPointX.transform.rotation = Quaternion.Lerp(rotationPointX.transform.rotation, lookRotation, 0.5f);
        }


    }
}