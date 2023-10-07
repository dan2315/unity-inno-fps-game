using System;
using UnityEngine;

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
        
        
        private Rigidbody _rigidbody;
        private bool _isGrounded;
        private float _dashCooldown;
        private Vector3 _desiredVelocity;

        public Weapon Weapon => weapon;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            CooldownDash();
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
            Jump();
            CustomGravity();
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

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Dash(moveDirection);
            }
        }
        
        private void Jump()
        {
            if (Input.GetKey(KeyCode.Space) && _isGrounded)
            {
                _isGrounded = false;
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void Dash(Vector3 direction)
        {
            if (_dashCooldown >= dashReplenishTime / 2)
            {
                _rigidbody.AddForce(direction * dashStrength, ForceMode.Impulse);
                _dashCooldown -= dashReplenishTime/2;
            }
        }
        
        private void CooldownDash()
        {
            if (_dashCooldown < dashReplenishTime)
            {
                _dashCooldown += Time.deltaTime;
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
    }
}
