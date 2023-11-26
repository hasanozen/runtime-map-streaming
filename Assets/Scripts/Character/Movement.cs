using UnityEngine;

namespace Character
{
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
            if (moveForwardAutomatically)
            {
                controller.Move(_playerTransform.forward * (1f * (speed * Time.deltaTime)));
                return;
            }
            
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            var move = _playerTransform.right * x + _playerTransform.forward * z;
            controller.Move(move * (speed * Time.deltaTime));
        }

        private void FixedUpdate()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_isGrounded && _velocity.y < 0f) _velocity.y = -2f;
            
            _velocity.y += gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
        }
    }
}