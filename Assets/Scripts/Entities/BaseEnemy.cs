using System;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseEnemy : Entity
    {
        protected NavMeshAgent NavAgent;
        protected Transform Transform;
        public GameObject BoomParticle;
        public int points = 20;

        public override void TakeDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Kill();
            }
        }

        protected void Kill()
        {
            Instantiate(BoomParticle, transform.position, new Quaternion(0, 0, 0, 0));
            Game.Instance.points += points;
            Destroy(gameObject);
        }

        protected virtual void Awake()
        {
            NavAgent = GetComponent<NavMeshAgent>();
            Transform = GetComponent<Transform>();
        }
    }
}