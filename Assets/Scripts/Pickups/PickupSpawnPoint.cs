using Unity.Mathematics;
using UnityEngine;

namespace Pickups
{
    public class PickupSpawnPoint : MonoBehaviour
    {
        [SerializeField] private float spawnCooldown;
        [SerializeField] private Pickup pickupPrefab;
        [SerializeField] private Transform visualsToRotate;

        private Pickup _presentedPickup;
        private float _cooldownTimer;

        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            visualsToRotate.rotation *= Quaternion.Euler(Vector3.up * 1);
            SpawnOnCooldown();
            Cooldown();
        }

        private void SpawnOnCooldown()
        {
            if (_cooldownTimer >= spawnCooldown && _presentedPickup == null) Spawn();
        }

        private void Cooldown()
        {
            if (_cooldownTimer < spawnCooldown && _presentedPickup == null) _cooldownTimer += Time.deltaTime;
        }

        private void Spawn()
        {
            _presentedPickup = Instantiate(pickupPrefab, transform.position, quaternion.identity, transform);
            _cooldownTimer = 0;
        }
    }
}