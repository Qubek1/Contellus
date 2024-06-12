using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EnemyAI
{
    /// <summary>
    ///     AI Actor
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIActor : MonoBehaviour
    {
        private IEnemyBehaviour _currentBehaviour;
        private NavMeshAgent _navMeshAgent;

        /// <summary>
        ///     Backend used to specifically follow Player.
        /// </summary>
        private class PlayerFollowBackend : FollowBehaviour.IFollowController
        {
            private GameObject _player;
            private readonly Transform _playerTransform;

            public PlayerFollowBackend(GameObject player)
            {
                _player = player;
                _playerTransform = player.GetComponent<Transform>();
            }

            public Vector3 GetTargetDestination()
            {
                return _playerTransform.position;
            }

            public void OnComplete()
            {
            }

            public void OnDestination(float dest)
            {
                if (dest < 2)
                {
                    
                }
            }
        }

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _currentBehaviour = new IdleBehaviour();
        }

        private void Update()
        {
            _currentBehaviour.Update();
        }

        public void FollowPlayer(GameObject player)
        {
            _currentBehaviour = new FollowBehaviour(gameObject, _navMeshAgent,
                new PlayerFollowBackend(player));
        }
    }
}