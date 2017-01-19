using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Allign : AgentBehaviour
    {
        [Tooltip("Raise In Case of Jitter movement")]
        public float targetRadius = 3;
        public float slowRadius = 6;
        public float timeToTarget = 0.1f;

        public override Steering GetSteering()
        {
            return GetSteering(this.Target, this.Agent);
        }
        public Steering GetSteering(GameObject target, IAgent agent)
        {
            return GetSteering(target.GetComponent<IAgent>().GetOrientation(), agent);
        }
        public Steering GetSteering(float targetOrientation, IAgent agent)
        {
            return GetSteering(targetOrientation, agent, targetRadius, slowRadius, timeToTarget);
        }
        public static Steering GetSteering(float targetOrientation, IAgent agent, float targetRadius, float slowRadius, float timeToTarget)
        {
            Steering steering = new Steering();

            float rotation = targetOrientation - agent.GetOrientation();
            rotation = MapToRange(rotation);
            float rotationSize = Mathf.Abs(rotation);
            if (rotationSize < targetRadius)
                return steering;
            float targetRotation;
            if (rotationSize > slowRadius)
                targetRotation = agent.GetMaxRotation();
            else
                targetRotation = agent.GetMaxRotation() * rotationSize / slowRadius;
            targetRotation *= rotation / rotationSize;
            steering.angular = targetRotation - agent.GetRotation();
            steering.angular /= timeToTarget;
            float angularAccel = Mathf.Abs(steering.angular);
            if (angularAccel > agent.GetMaxAngularAccel())
            {
                steering.angular /= angularAccel;
                steering.angular *= agent.GetMaxAngularAccel();
            }
            return steering;
        }
    }
}
