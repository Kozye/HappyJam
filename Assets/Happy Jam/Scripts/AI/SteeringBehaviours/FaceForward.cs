using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class FaceForward : Allign
    {
        protected float _targetOrientation = 0;
        public override void Awake()
        {
            base.Awake();
            targetRequired = false;
        }

        public override Steering GetSteering()
        {
            Vector3 velocity = Agent.GetVelocity();

            if (velocity.magnitude <= 0.0001f)
                return new Steering();

            if (Agent is Agent2D)
                _targetOrientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            else
                _targetOrientation = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

            return base.GetSteering(_targetOrientation, Agent);
        }
    }
}
