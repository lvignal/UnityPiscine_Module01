using UnityEngine;
using System;
using Module01.Teleportation;

namespace Module01.Character
{
    public class PlayerController : MonoBehaviour
    {
        public KeyCode FocusKeyCode;
        [SerializeField] private GameObject _correspondingExit;
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;

        private bool _isActive = false;

        private bool _isJumping;
        private bool _isOnGround;
        private bool _isNearWall;
        private bool _isOnPlatform;
        private bool _hasValidatedExit;
        
        private Rigidbody _rigidBody;
        private LayerMask _layerMaskToDetectWalls;

        private float _halfHeight;
        private float _halfWidth;

        public Action<bool> OnExitReached;
        public Action OnPlayerDeath;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _isNearWall = false;
            _halfHeight = transform.localScale.y / 2;
            _halfWidth = transform.localScale.x / 2;
            int layerToExclude = LayerMask.NameToLayer("Default");
            _layerMaskToDetectWalls = ~(1 << layerToExclude);
        }

        private void FixedUpdate()
        {
            if (!_isActive)
            {
                if (!_isOnGround)
                    _rigidBody.AddForce(Vector3.down, ForceMode.Force);
                return;
            }
            
            _isNearWall = IsPlayerNearWall();
            
            HandlePlayerLateralMove();
            HandlePlayerPhysics();
        }
        
        private void HandlePlayerLateralMove()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 move = new Vector3(horizontalInput * _speed * Time.deltaTime, 0, 0);
            
            // avoid moving if the character is near a wall
            if (_isNearWall && (horizontalInput > 0 && Physics.Raycast(transform.position, transform.right, _halfWidth + 0.05f)
                               || horizontalInput < 0 && Physics.Raycast(transform.position, -transform.right, _halfWidth + 0.05f)))
                move = Vector3.zero;
            _rigidBody.MovePosition(_rigidBody.position + move);
        }

        private void HandlePlayerPhysics()
        {
            // jump
            if (_isJumping && _isOnGround)
            {
                _rigidBody.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
                _isOnGround = false;
            }
            
            // make the jump fall faster
            if (_rigidBody.velocity.y < 0 && !_isOnGround)
            {
                float fallMultiplier = 3.0f;
                _rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            
            // slide down the wall and avoid wall jump
            if (_isNearWall && !_isOnGround)
                _rigidBody.AddForce(Vector3.down, ForceMode.Force);
        }
        
        private void Update()
        {
            if (!_isActive)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space) && !_isJumping && !_isNearWall)
                _isJumping = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            string objectTag = other.gameObject.tag;
        
            // check if the character is on the ground (avoid wall jump)
            if ((objectTag == "Ground" || objectTag == "Character" || objectTag == "Platform") && IsGrounded())
            {
                _isOnGround = true;
                _isJumping = false;
                
                if (objectTag == "Platform")
                   transform.SetParent(other.transform);
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            string objectTag = other.gameObject.tag;
            
            if ((objectTag == "Ground" || objectTag == "Character" || objectTag == "Platform") && !IsGrounded())
                _isOnGround = false;
            
            if (objectTag == "Platform")
                transform.SetParent(null);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject != _correspondingExit)
                return;
            
            // check if the character is totally in the exit
            if (IsCenteredInObject(other.transform) && !_hasValidatedExit)
            {
                _hasValidatedExit = true;
                OnExitReached?.Invoke(true);
            }

            else if (!IsCenteredInObject(other.transform) && _hasValidatedExit)
            {
                _hasValidatedExit = false;
                OnExitReached?.Invoke(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<TeleportationEnter>())
                transform.position = other.GetComponent<TeleportationEnter>().Out.position;
            
            if (other.gameObject.tag == "DeadlyObject" && (other.gameObject.layer == gameObject.layer || other.gameObject.layer == default))
                OnPlayerDeath?.Invoke();
        }

        /// <summary>
        /// Set the character active or not. If active, the camera is centered on it and it can be controlled
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            // avoid that characters push each other
            _rigidBody.constraints = isActive ? RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ 
                : RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        /// <summary>
        /// Detects if the bottom of the cube is touching the ground by launching 3 rays (center, right, left)
        /// </summary>
        /// <returns></returns>
        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, _halfHeight + 0.1f)
                   || Physics.Raycast(
                       new Vector3(transform.position.x + _halfWidth, transform.position.y, transform.position.z),
                       Vector3.down, _halfHeight + 0.1f)
                   || Physics.Raycast(
                       new Vector3(transform.position.x - _halfWidth, transform.position.y, transform.position.z),
                       Vector3.down, _halfHeight + 0.1f);
        }

        /// <summary>
        /// Check if the character is inside the object, correctly centered
        /// </summary>
        /// <param name="objectTransform"></param>
        /// <returns></returns>
        public bool IsCenteredInObject(Transform objectTransform)
        {
            return transform.position.x > objectTransform.position.x - 0.05
                   && transform.position.x < objectTransform.position.x + 0.05;
        }

        /// <summary>
        /// Detects if the player is near a wall (on his left or right), by launching 6 rays (top, center, bottom) on each side.
        /// Detects all layers except the default one so we don't take the exit in account
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerNearWall()
        {
            Vector3 bottom= new Vector3(transform.position.x, transform.position.y - _halfHeight + 0.1f, transform.position.z);
            Vector3 top = new Vector3(transform.position.x, transform.position.y + _halfHeight, transform.position.z);

            return Physics.Raycast(transform.position, transform.right, _halfWidth + 0.05f, _layerMaskToDetectWalls) ||
                   Physics.Raycast(transform.position, -transform.right, _halfWidth + 0.05f, _layerMaskToDetectWalls) ||
                   Physics.Raycast(bottom, transform.right, _halfWidth + 0.05f, _layerMaskToDetectWalls) ||
                   Physics.Raycast(bottom, -transform.right, _halfWidth + 0.05f, _layerMaskToDetectWalls) ||
                   Physics.Raycast(top, transform.right, _halfWidth + 0.05f, _layerMaskToDetectWalls) ||
                   Physics.Raycast(top, -transform.right, _halfWidth + 0.05f, _layerMaskToDetectWalls);
        }
    }
}