using Module01.Character;
using UnityEngine;

namespace Module01.Platform
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private Vector3 _moveDirection;
        [SerializeField] private float _stopPositionY = -1;
        [SerializeField] private float _speed = 2.0f;
        
        private bool _isMoving = false;
        private bool _isStopped = false;
        private int _whiteLayer;

        private void Start()
        {
            _whiteLayer = LayerMask.NameToLayer("White");
        }

        private void Update()
        {
            if (_isMoving && !_isStopped)
                transform.Translate(_moveDirection.normalized * Time.deltaTime * _speed);
            
            // stop the platform if it reaches the stop position
            if (_isMoving && _stopPositionY != -1f && transform.localPosition.y >= _stopPositionY)
            {
                _isMoving = false;
                _isStopped = true;
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.tag != "Character" || _isStopped)
                return;
            
            if (gameObject.layer != _whiteLayer && other.gameObject.layer != gameObject.layer)
                return;
            
            PlayerController characterController = other.gameObject.GetComponent<PlayerController>();
            if (characterController != null && characterController.IsCenteredInObject(gameObject.transform))
                _isMoving = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Ground")
            {
                _isMoving = false;
                _isStopped = true;
            }
        }
    }
}
