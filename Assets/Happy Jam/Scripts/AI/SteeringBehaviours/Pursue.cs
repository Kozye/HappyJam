using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Pursue : Seek
    {
        public float maxPrediction;
        protected Vector3 _targetPosition;
        protected IAgent _targetAgent;

        public override Steering GetSteering()
        {
            if (_targetAgent == null) _targetAgent = Target.GetComponent<IAgent>();
            Vector3 direction = Target.transform.position - transform.position;
            float distance = direction.magnitude;
            float speed = Agent.GetVelocity().magnitude;
            float prediction;
            if (speed <= distance / maxPrediction)
                prediction = maxPrediction;
            else
                prediction = distance / speed;
            _targetPosition = Target.transform.position;
            _targetPosition += _targetAgent.GetVelocity() * prediction;

            return base.GetSteering(_targetPosition);
        }
    }
}
