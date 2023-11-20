using System;
using DG.Tweening;
using Gamemode;
using UnityEngine;
using static UnityEngine.Screen;

namespace Character
{
    public class PlayableCharacter : Character
    {
        [SerializeField] private float sensitivity;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float dashStrength;
        [SerializeField] private float dashReplenishTime;
        [SerializeField] private float gravityForce;
        [SerializeField] private Weapon weapon;
        [SerializeField] private Camera camera;
        [SerializeField] private Transform rotationBone;

        [SerializeField] private AudioSource hurtSound;
        [SerializeField] private AudioSource footstepsSound;
        [SerializeField] private AudioSource jumpSound;
        [SerializeField] private AudioSource pickupSound;
        
       

        private float _jumpCooldown;
        private Rigidbody _rigidbody;
        private bool _isGrounded;
        private int _jumpCount = 2;
        private bool _dashState;
        private float _dashCooldown;
        private Vector3 _desiredVelocity;

        public Weapon Weapon => weapon;

        public Action<float> OnDashCooldown;

        public static PlayableCharacter Instance => _instance;
        private static PlayableCharacter _instance;

        protected override void Awake()
        {
            base.Awake();
            _instance = this;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            HP.DamageTaken += hurtSound.Play;
        }

        private void Update()
        {
            CooldownDash();
        }

        private void FixedUpdate()
        {
            if (GameController.Instance.IsPlayerDead) return;

            Move();
            Rotate();
            Jump();
            CustomGravity();
            Shoot();
        }

        private void Rotate()
        {
            Vector3 mouseMovementDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            _rigidbody.MoveRotation(transform.rotation *
                                    Quaternion.Euler(Vector3.up * mouseMovementDelta.x * sensitivity));
            
            rotationBone.rotation *= Quaternion.Euler(Vector3.left * mouseMovementDelta.y * sensitivity);
            var tempRot = rotationBone.localEulerAngles;
            tempRot.z = Mathf.Clamp(tempRot.z, -60, 60);

            weapon.RotateVerticallyAndNotVertically(rotationBone.rotation, mouseMovementDelta);
        }

        private void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
            _rigidbody.velocity = moveDirection * (speed + (_dashState ? dashStrength : 0)) +
                                  _rigidbody.velocity.y * Vector3.up;
            
            if (_isGrounded && moveDirection.magnitude >= 0.01f) footstepsSound.Play();
            else footstepsSound.Stop();
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dash(moveDirection);
            }
        }

        private void Jump()
        {
            if (Input.GetKey(KeyCode.Space) && _jumpCount > 0 && _jumpCooldown >= 0.3f)
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _jumpCooldown = 0;
                DOTween.Sequence().AppendInterval(0.1f).OnComplete(() =>
                {
                    _isGrounded = false;
                    _jumpCount--;
                    jumpSound.Play();
                });
            }

            _jumpCooldown += Time.deltaTime;
        }

        private void Dash(Vector3 direction)
        {
            if (_dashCooldown >= dashReplenishTime / 2)
            {
                _dashState = true;
                DOTween.Sequence().AppendInterval(0.25f).OnComplete(() => _dashState = false);
                _dashCooldown -= dashReplenishTime / 2;
                jumpSound.Play();
            }
        }

        private void Shoot()
        {
            Vector3 targetPosition;
            Ray ray = camera.ScreenPointToRay(new Vector2(width / 2, height / 2));
            if (Physics.Raycast(ray, out var hit))
            {
                targetPosition = hit.point;
            }
            else
            {
                var expectedPoint = ray.GetPoint(100);
                targetPosition = expectedPoint;
            }

            if (GameController.Instance.IsPreparationStage) return;
            weapon.Shoot(Input.GetKey(KeyCode.Mouse0), targetPosition);
        }

        private void CooldownDash()
        {
            if (_dashCooldown < dashReplenishTime)
            {
                _dashCooldown += Time.deltaTime;
                OnDashCooldown?.Invoke(_dashCooldown);
            }
        }

        public void PlayPickupSound()
        {
            pickupSound.Play();
        }

        private void OnCollisionEnter(Collision other)
        {
            _isGrounded = true;
            _jumpCount = 2;
        }

        private void CustomGravity()
        {
            _rigidbody.AddForce(Vector3.down * gravityForce);
        }

        protected override void Death()
        {
            GameController.Instance.HandleLose();
        }
    }
}