using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        private void Start()
        {
            Health = baseMaxHealth;
        }

        public float Health { get; protected set; }

        public float baseMaxHealth;

        public abstract void TakeDamage(float damage);
    }
}