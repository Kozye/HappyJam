using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Face Forward")]
    [Help("Steering Behaviour Face Forward")]
    public class FaceForward : Allign
    {
        protected float _targetOrientation = 0;
        private IAgent _targetAgent;

        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Vector3 velocity = agent.GetVelocity();

            if (velocity.magnitude <= 0.0001f)
                return new Steering();

            if (agent is Agent2D)
                _targetOrientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            else
                _targetOrientation = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

            return base.GetSteering(_targetOrientation, agent);
        }
    }
}
