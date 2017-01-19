using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{
    [Action("Steering Behaviour/Wander")]
    [Help("Steering Behaviour Wander. Target Should be null")]
    public class Wander : Face
    {
        [InParam("offset")]
        public float offset;
        [InParam("radius")]
        public float radius;
        [InParam("rate")]
        public float rate;

        private bool _initialized = false;

        public override void OnStart()
        {
            base.OnStart();
            if (gameObject && !target)
            {
                target = new GameObject();
                target.transform.position = _transform.position;
            }
        }
        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            float wanderOrientation = Random.Range(-1.0f, 1.0f) * rate;
            float targetOrientation = wanderOrientation + agent.GetOrientation();
            if (agent is Agent2D)
            {
                Vector3 orientationVec = AI.AgentBehaviour.GetOriAsVec2D(agent.GetOrientation());
                Vector3 targetPosition = (offset * orientationVec) + _transform.position;
                targetPosition = targetPosition + (AI.AgentBehaviour.GetOriAsVec2D(targetOrientation) * radius);
                target.transform.position = targetPosition;
            }
            else
            {
                Vector3 orientationVec = AI.AgentBehaviour.GetOriAsVec(agent.GetOrientation());
                Vector3 targetPosition = (offset * orientationVec) + _transform.position;
                targetPosition = targetPosition + (AI.AgentBehaviour.GetOriAsVec(targetOrientation) * radius);
                target.transform.position = targetPosition;
            }
            steering = base.GetSteering();
            steering.linear = target.transform.position - _transform.position;
            steering.linear.Normalize();
            steering.linear *= agent.GetMaxAccel();
            return steering;
        }
    }
}
