using UnityEngine;

namespace Character
{
    /// <summary>
    /// Handles the basic movement of the player.
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed = 12f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private bool moveForwardAutomatically;

        private Transform _playerTransform;
        private Vector3 _velocity;
        private bool _isGrounded;

        private void Awake()
        {
            _playerTransform = transform;
        }

        private void Update()
        {
            HandleMovementInput();
        }
        
        private void FixedUpdate()
        {
            CheckGrounded();
            ApplyGravity();
            MoveWithVelocity();
        }

        /// <summary>
        /// Handles the movement input.
        /// </summary>
        private void HandleMovementInput()
        {
            if (moveForwardAutomatically) MoveForwardAutomatically();
            else MoveBasedOnInput();
        }

        /// <summary>
        /// Moves the player forward automatically with defined constant speed.
        /// </summary>
        private void MoveForwardAutomatically()
        {
            controller.Move(_playerTransform.forward * (1f * (speed * Time.deltaTime)));
        }

        /// <summary>
        /// Gets the input from the player and moves the player based on the input.
        /// </summary>
        private void MoveBasedOnInput()
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            var move = _playerTransform.right * x + _playerTransform.forward * z;
            controller.Move(move * (speed * Time.deltaTime));
        }

        /// <summary>
        /// Checks if the player is grounded. If the player is grounded, the y velocity is set to -2f.
        /// </summary>
        private void CheckGrounded()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_isGrounded && _velocity.y < 0f)
                _velocity.y = -2f;
        }

        /// <summary>
        /// Applies gravity to the player.
        /// </summary>
        private void ApplyGravity()
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        /// <summary>
        /// Moves player on the y axis with the velocity. This method simulates gravitational pull.
        /// </summary>
        private void MoveWithVelocity()
        {
            controller.Move(_velocity * Time.deltaTime);
        }
    }
}
