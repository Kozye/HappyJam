using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{
    [Action("Steering Behaviour/Evade")]
    [Help("Steering Behaviour Evade that requires Agent component")]
    public class Evade : Flee
    {
        [InParam("Max Prediction")]
        public float maxPrediction;

        protected Vector3 _targetPosition;
        protected IAgent _targetAgent;

        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            if (_transform)
                SetSteering();
            return TaskStatus.RUNNING;
        }
        public override Steering GetSteering()
        {
            if (_targetAgent == null) _targetAgent = target.GetComponent<IAgent>();
            Vector3 direction = target.transform.position - _transform.position;
            float distance = direction.magnitude;
            float speed = agent.GetVelocity().magnitude;
            float prediction;
            if (speed <= distance / maxPrediction)
                prediction = maxPrediction;
            else
                prediction = distance / speed;
            _targetPosition = target.transform.position;
            _targetPosition += _targetAgent.GetVelocity() * prediction;

            return base.GetSteering(_targetPosition);
        }
    }
}
