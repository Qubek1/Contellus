using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Weapons;
using Debug = System.Diagnostics.Debug;

namespace Entities
{
    public class Player : Entity
    {
        [FormerlySerializedAs("damage")] public float baseDamage;

        [Header("Shooting")] [SerializeField] public float timeBetweenShots;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private GameObject attackPoint;

        [SerializeField] [FormerlySerializedAs("BulletPrefab")]
        private GameObject bulletPrefab;
        [SerializeField] [FormerlySerializedAs("BulletPrefab")]
        private GameObject bulletParticles;

        [Header("Movement")] public float moveSpeed = 4f;
        
        [HideInInspector] public bool tripleShot = false;
        [HideInInspector] public float healAmount = 0;
        
        private bool _readyToShoot;
        private Vector3 _cameraForward, _cameraRight;
        private Camera _camera;

        public GameObject particlePrefab;

        private void Awake()
        {
            _readyToShoot = true;

            // Setup movement
            _camera = Camera.main;
            Debug.Assert(_camera != null, "Camera.main != null");
            _cameraForward = _camera.transform.forward;
            _cameraForward.y = 0;
            _cameraForward = Vector3.Normalize(_cameraForward);
            _cameraRight = Quaternion.Euler(new Vector3(0, 90, 0)) * _cameraForward;
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                Move();
            }

            Aim();

            if (_readyToShoot && Input.GetAxisRaw("Fire1") > 0)
            {
                Shoot();
            }

            Health += healAmount * Time.deltaTime;

            if (Health > baseMaxHealth)
            {
                Health = baseMaxHealth;
            }
        }

        private void Shoot()
        {
            _readyToShoot = false;

            void CreateBullet(Vector3 position, Quaternion rotation)
            {
                var projectileInstance = Instantiate(bulletPrefab, position, rotation);
                var bulletConfig = projectileInstance.GetComponent<BulletScript>();
                bulletConfig.bulletSpeed = bulletSpeed;
                bulletConfig.bulletDamage = baseDamage;
            }

            void CreateParticles(Vector3 position, Quaternion rotation)
            {
                var projectileInstance = Instantiate(bulletParticles, position, rotation);
            }

            var attackTransform = attackPoint.transform;
            var direction = attackTransform.position + attackTransform.forward;
            // Spawn bullet
            CreateBullet(direction, attackTransform.rotation);
            // Spawn Particles
            CreateParticles(direction, attackTransform.rotation);

            // If triple shot, spawn additional 2
            if (tripleShot)
            {
                var rotation = attackTransform.rotation;
                CreateBullet(direction, rotation * Quaternion.Euler(Vector3.up * 30));
                CreateBullet(direction, rotation * Quaternion.Euler(Vector3.down * 30));
            }

            StartCoroutine(ResetShot());
        }

        private IEnumerator ResetShot()
        {
            yield return new WaitForSecondsRealtime(timeBetweenShots);
            _readyToShoot = true;
        }

        private void Move()
        {
            var rightMovement = _cameraRight * (moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey"));
            var forwardMovement = _cameraForward * (moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey"));

            if (rightMovement.magnitude != 0 && forwardMovement.magnitude != 0)
            {
                transform.position += (rightMovement + forwardMovement) / Mathf.Sqrt(2);
            }
            else
            {
                transform.position += rightMovement + forwardMovement;
            }
        }

        private void Aim()
        {
            var planeNormal = new Vector3(0f, 1f, 0f);
            var planeCenter = new Vector3(0f, transform.position.y, 0f);
            var cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
            var lineOrigin = cameraRay.origin;
            var lineDirection = cameraRay.direction;

            var difference = planeCenter - lineOrigin;
            var denominator = Vector3.Dot(lineDirection, planeNormal);
            var t = Vector3.Dot(difference, planeNormal) / denominator;

            var planeIntersection = lineOrigin + lineDirection * t;

            transform.LookAt(planeIntersection);
        }

        public override void TakeDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Kill();
            }
        }

        public void ResetPowerUps()
        {
            healAmount = 0;
            tripleShot = false;
            timeBetweenShots = 0.2f;
            moveSpeed = 8;
            baseDamage = 10;
        }


        private void Kill()
        {
            /*IEnumerator ReloadScene()
            {
                yield return new WaitForSecondsRealtime(5);
                var currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }

            Instantiate(particlePrefab, transform.position, Quaternion.identity);
            StartCoroutine(ReloadScene());*/
            GameOver.singleton.gameObject.SetActive(true);
            Health = 0;
            gameObject.SetActive(false);
        }
    }
}