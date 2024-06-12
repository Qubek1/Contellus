using System;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class BulletScript : MonoBehaviour
    {
        [FormerlySerializedAs("bulletspeed")] public float bulletSpeed;
        [FormerlySerializedAs("bulletdamage")] public float bulletDamage;
        [FormerlySerializedAs("boom")] public GameObject BoomParticle;

        private Transform _bulletTransform;

        private void Awake()
        {
            _bulletTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            _bulletTransform.position += _bulletTransform.forward * (bulletSpeed * Time.deltaTime);

            if (transform.position.magnitude > 1000f)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var collidedEntity = collision.collider.GetComponent<Entity>();
            
            // If this is actually an entity
            if (collidedEntity != null)
            {
                collidedEntity.TakeDamage(bulletDamage);
                Instantiate(BoomParticle, transform.position, new Quaternion(0,0,0,0));
                Destroy(gameObject);
            }
        }
    }
}