using System;
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

        private float _jumpCooldown;
        private Rigidbody _rigidbody;
        private bool _isGrounded;
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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            CooldownDash();
            Move();
        }

        private void FixedUpdate()
        {
            Rotate();
            Jump();
            CustomGravity();
            Shoot();
        }

        private void Rotate()
        {
            Vector3 mouseMovementDelta = new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));

            _rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(Vector3.up * mouseMovementDelta.x * sensitivity));
            rotationBone.rotation *= Quaternion.Euler(Vector3.left * mouseMovementDelta.y * sensitivity);
            weapon.RotateVerticallyAndNotVertically(rotationBone.rotation, mouseMovementDelta);
        }
        
        private void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
            _rigidbody.velocity = moveDirection * speed + _rigidbody.velocity.y * Vector3.up;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dash(moveDirection);
            }
        }
        
        private void Jump()
        {
            if (Input.GetKey(KeyCode.Space) && _isGrounded && _jumpCooldown >= 0.3f)
            {
                _isGrounded = false;
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _jumpCooldown = 0;
            }

            _jumpCooldown += Time.deltaTime;
        }

        private void Dash(Vector3 direction)
        {
            if (_dashCooldown >= dashReplenishTime / 2)
            {
                _rigidbody.AddForce(direction * dashStrength, ForceMode.Impulse);
                _dashCooldown -= dashReplenishTime/2;
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

        private void OnCollisionEnter(Collision other)
        {
            _isGrounded = true;
        }
        
        private void CustomGravity()
        {
            _rigidbody.AddForce(Vector3.down * gravityForce);
        }

        protected override void Death()
        {
            
        }
    }
}
