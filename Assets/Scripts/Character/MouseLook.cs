using UnityEngine;

namespace Character
{
    /// <summary>
    /// Takes the mouse position as input and rotates the player based on the input.
    /// </summary>
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private Transform playerBody;
    
        private float _rotationX;
    
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    
        private void Update()
        {
            Look();
        }

        /// <summary>
        /// Gets the mouse input and rotates the player based on the input.
        /// Clamps the rotation between -90 and 90 degrees on the x axis.
        /// </summary>
        private void Look()
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            _rotationX -= mouseY;
            _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
