using System;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Screen;

namespace Character
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float reloadTime = 2;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private Camera camera;
        [SerializeField] private ParticleSystem shootingVfx;

        [SerializeField] private Transform visuals;
        [SerializeField] private Transform magazine;
        [SerializeField] private Transform rightArmPivot;
        [SerializeField] private Transform leftArmPivot;

        private float _initialRightArmPivotHeight;
        private float _initialLeftArmPivotHeight;
        private float _initialMagazineHeight;
        private float _initialRelativeHeight;

        private float _fireRate = 5; //     bullets / sec

        private float _fireCooldown;

        private int _maxAmmo = 180;
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
            
            _initialRelativeHeight = transform.localPosition.y;
            _initialRightArmPivotHeight = rightArmPivot.localPosition.y;
            _initialLeftArmPivotHeight = leftArmPivot.localPosition.y;
            _initialMagazineHeight = magazine.localPosition.y;
        }

        private void Update()
        {
            // Shoot();
            ForceReload();
            Modifier.ProcessTime();
        }

        public void Shoot(Quaternion shootDirection)
        {
            if (_isReloading) return;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (_ammoInMagazine <= 0)
                {
                    Reload();
                    if (shootingVfx.isPlaying) shootingVfx.Stop();
                }
                else if (_fireCooldown >= 1 / _fireRate)
                {
                    Quaternion rotation;
                    Ray ray = camera.ScreenPointToRay(new Vector2(width / 2, height / 2));
                    if (Physics.Raycast(ray, out var hit))
                    {
                        rotation = Quaternion.LookRotation(hit.point - ray.origin, Vector3.up);
                    }
                    else
                    {
                        rotation = Quaternion.LookRotation((ray.origin + ray.direction * 10) - ray.origin, Vector3.up);
                    }


                    Instantiate(bulletPrefab, shootingPoint.position, rotation, _bulletParent.transform);
                    _ammoInMagazine -= 1;
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
        }

        public void RotateVerticallyAndNotVertically(Quaternion rotation, Vector3 mouseMovementDelta)
        {
            var deviation = rotation.eulerAngles.x <= 180 ? -rotation.eulerAngles.x : 360 - rotation.eulerAngles.x;
            
            var modifiedRotation = rotation.eulerAngles;
            modifiedRotation.x = deviation * -3f;
            transform.eulerAngles = modifiedRotation;
            
            var modifiedLocalRotation = rotation.eulerAngles;
            modifiedLocalRotation.y = mouseMovementDelta.x * -4;
            transform.localRotation = Quaternion.Lerp(transform.localRotation ,Quaternion.Euler(modifiedLocalRotation), 0.25f); 

            var modifiedPosition = transform.localPosition;
            modifiedPosition.y = _initialRelativeHeight + deviation * 0.01f;
            
            var modifiedRightArmPosition = rightArmPivot.localPosition;
            modifiedRightArmPosition.y = _initialRightArmPivotHeight + deviation * 0.01f;
            
            var modifiedLeftArmPosition = leftArmPivot.localPosition;
            modifiedLeftArmPosition.y = _initialLeftArmPivotHeight + deviation * 0.01f;

            rightArmPivot.localPosition = modifiedRightArmPosition; 
            leftArmPivot.localPosition = modifiedLeftArmPosition;
            transform.localPosition = modifiedPosition;
        }
    }
}