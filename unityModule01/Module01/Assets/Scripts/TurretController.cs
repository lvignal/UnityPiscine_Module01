using UnityEngine;

namespace Module01.Turret
{
    public class TurretController : MonoBehaviour
    {
        [SerializeField] private GameObject _characterToTarget;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _shootingInterval;
        [SerializeField] private Transform _turretHead;
        [SerializeField] private Transform _shootingStart;

        private float _timer = 0;
        private float _bulletSpeed = 1.8f;

        private void Start()
        {
            if (gameObject.GetComponent<Renderer>().material.color != _characterToTarget.GetComponent<Renderer>().material.color)
            {
                Debug.LogError("Turret and character to target have different colors");
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            
            if (_timer >= _shootingInterval)
            {
                LaunchBullet();
                _timer = 0;
            }
        }

        private void FixedUpdate()
        {
            _turretHead.LookAt(_characterToTarget.transform);
            _turretHead.Rotate(0, 180, 0);
            _shootingStart.LookAt(_characterToTarget.transform);
            _shootingStart.Rotate(0, 180, 0);
        }
        
        private void LaunchBullet()
        {
            GameObject bullet = Instantiate(_bulletPrefab, _shootingStart.position, _shootingStart.transform.rotation);
            bullet.layer= _characterToTarget.layer;
            Vector3 direction = _characterToTarget.transform.position - _shootingStart.position;
            bullet.GetComponent<Rigidbody>().AddForce(direction * _bulletSpeed, ForceMode.Impulse);
            Destroy(bullet, 5f);
        }
    }
}