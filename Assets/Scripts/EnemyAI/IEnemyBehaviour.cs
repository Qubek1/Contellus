using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    public interface IEnemyBehaviour
    {
        void Update();
    }

    /// <summary>
    ///     A state when enemy does not do anything, simply because there is no goal to chase.
    ///     Can be used for example, when the game ends, and we want to setup a end screen
    /// </summary>
    public class IdleBehaviour : IEnemyBehaviour
    {
        public void Update()
        {
            // Do nothing
        }
    }
    
    /// <summary>
    ///     Makes the agent follow a specified target. You can listen
    /// to this state emitted events using <see cref="IFollowController"/>
    /// </summary>
    public class FollowBehaviour : IEnemyBehaviour
    {
        private readonly GameObject _entity;
        private readonly NavMeshAgent _agent;
        private readonly IFollowController _controller;
        private readonly Transform _entityTransform;

        public FollowBehaviour(GameObject entity, NavMeshAgent agent, IFollowController controller)
        {
            _entity = entity;
            _agent = agent;
            _controller = controller;
            _entityTransform = entity.GetComponent<Transform>();
        }

        public void Update()
        {
            var targetDestination = _controller.GetTargetDestination();
            _agent.destination = targetDestination;

            var distance = Vector3.Distance(_entityTransform.position, targetDestination);
            _controller.OnDestination(distance);
        }

        /// <summary>
        ///     Frontend for reacting to follow state events
        /// </summary>
        public interface IFollowController
        {
            /// <summary>
            ///     Provide a target to follow
            /// </summary>
            /// <returns></returns>
            Vector3 GetTargetDestination();

            /// <summary>
            ///     When target is reached
            /// </summary>
            void OnComplete();

            /// <summary>
            ///     Triggers event each frame with destination towards target
            /// </summary>
            void OnDestination(float dest);
        }
    }
    
    
}