using System;
using DG.Tweening;
using Gamemode;
using Pickups;
using UnityEngine;

namespace Character
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float reloadTime = 2;
        [SerializeField] private float fireRate = 5; //     bullets / sec
        
        
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Bullet fireBulletPrefab;
        [SerializeField] private Bullet electricBulletPrefab;
        
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private ParticleSystem shootingVfx;
        [SerializeField] private AudioSource shotAudioSource;

        [SerializeField] private Transform visuals;
        [SerializeField] private Transform magazine;
        [SerializeField] private Transform rightArmPivot;
        [SerializeField] private Transform leftArmPivot;

        [SerializeField] private bool infiniteAmmo;
                

        private float _initialRightArmPivotHeight;
        private float _initialLeftArmPivotHeight;
        private float _initialMagazineHeight;
        private float _initialRelativeHeight;
        
        private float _fireCooldown;

        private int _maxAmmo = 90;
        private int _magazineSize = 30;

        private int _totalAmmo;
        private int _ammoInMagazine;

        private bool _isReloading;

        private GameObject _bulletParent;
        
        public Modifier Modifier = new();

        public Action<int, int> AmmoChanged;

        private void Start()
        {
            _bulletParent = new GameObject();
            _totalAmmo = 60;
            _ammoInMagazine = 30;
            
            
            if (visuals == null) return;
            _initialRelativeHeight = transform.localPosition.y;
            _initialRightArmPivotHeight = rightArmPivot.localPosition.y;
            _initialLeftArmPivotHeight = leftArmPivot.localPosition.y;
            _initialMagazineHeight = magazine.localPosition.y;
        }

        private void Update()
        {
            ForceReload();
            Modifier.ProcessTime();
        }

        public void Shoot(bool shootingCondition, Vector3 targetPosition)
        {
            if (_isReloading) return;
            if (shootingCondition)
            {
                if (_ammoInMagazine <= 0)
                {
                    Reload();
                    if (shootingVfx.isPlaying) shootingVfx.Stop();
                }
                else if (_fireCooldown >= 1 / fireRate)
                {
                    var prefabToSpawn = Modifier.Type switch
                    {
                        ModifierType.FireModifier => fireBulletPrefab,
                        ModifierType.ElectricalModifier => electricBulletPrefab,
                        _ => bulletPrefab
                    };

                    shotAudioSource.Play();
                    var bullet = Instantiate(prefabToSpawn, shootingPoint.position, Quaternion.LookRotation(targetPosition - shootingPoint.position) , _bulletParent.transform);
                    bullet.SetDamageModifier(Modifier.Type);
                    if (!infiniteAmmo) _ammoInMagazine -= 1;
                    AmmoChanged?.Invoke(_ammoInMagazine, _totalAmmo);
                    _fireCooldown = 0;
                }
                else
                {
                    if (!shootingVfx.isPlaying) shootingVfx.Play();
                }
            }
            else
            {
                if (shootingVfx.isPlaying) shootingVfx.Stop();
            }

            _fireCooldown += Time.deltaTime;
        }

        private void ForceReload()
        {
            if (Input.GetKey(KeyCode.R))
            {
                Reload();
            }
        }

        private void Reload()
        {
            if (_ammoInMagazine >= _magazineSize || _totalAmmo <= 0) return;
            _isReloading = true;
            if (shootingVfx.isPlaying) shootingVfx.Stop();

            AnimateReloading(CalculateAmmoAfterReloading);
        }

        private void CalculateAmmoAfterReloading()
        {
            int requiredAmmo = _magazineSize - _ammoInMagazine;
            if (_totalAmmo < requiredAmmo) requiredAmmo = _totalAmmo;
            _totalAmmo -= requiredAmmo;
            _ammoInMagazine += requiredAmmo;
            AmmoChanged?.Invoke(_ammoInMagazine, _totalAmmo);
            _isReloading = false;
        }

        private void AnimateReloading(Action onComplete)
        {
            if (visuals == null) return;

            DOTween.Sequence()
                .Append(visuals.DOLocalRotate(Vector3.forward * -30, 0.25f * reloadTime))
                .Append(magazine.DOLocalMoveY(_initialMagazineHeight - 1, 0.25f * reloadTime))
                .Join(leftArmPivot.DOLocalMoveY(_initialLeftArmPivotHeight - 1, 0.25f * reloadTime))
                .Append(magazine.DOLocalMoveY(_initialMagazineHeight, 0.25f * reloadTime))
                .Join(leftArmPivot.DOLocalMoveY(_initialLeftArmPivotHeight, 0.25f * reloadTime))
                .Append(visuals.DOLocalRotate(Vector3.forward * 0, 0.25f * reloadTime)
                .OnComplete(onComplete.Invoke));
        }

        public void AddBullets(int amount)
        {
            _totalAmmo += amount;
            if (_totalAmmo > _maxAmmo) _totalAmmo = _maxAmmo;
            AmmoChanged?.Invoke(_ammoInMagazine, _totalAmmo);
        }

        public void RotateVerticallyAndNotVertically(Quaternion rotation, Vector3 mouseMovementDelta)
        {
            var deviation = rotation.eulerAngles.x <= 180 ? -rotation.eulerAngles.x : 360 - rotation.eulerAngles.x;
            
            var modifiedRotation = rotation.eulerAngles;
            modifiedRotation.x = deviation * -1.5f;
            transform.eulerAngles = modifiedRotation;
            
            var modifiedLocalRotation = rotation.eulerAngles;
            modifiedLocalRotation.y = mouseMovementDelta.x * -4;
            transform.localRotation = Quaternion.Lerp(transform.localRotation ,Quaternion.Euler(modifiedLocalRotation), 0.25f); 

            var modifiedPosition = transform.localPosition;
            modifiedPosition.y = _initialRelativeHeight + deviation * 0.005f;
            
            var modifiedRightArmPosition = rightArmPivot.localPosition;
            modifiedRightArmPosition.y = _initialRightArmPivotHeight + deviation * 0.005f;
            
            var modifiedLeftArmPosition = leftArmPivot.localPosition;
            modifiedLeftArmPosition.y = _initialLeftArmPivotHeight + deviation * 0.005f;

            rightArmPivot.localPosition = modifiedRightArmPosition; 
            leftArmPivot.localPosition = modifiedLeftArmPosition;
            transform.localPosition = modifiedPosition;
        }
    }
}