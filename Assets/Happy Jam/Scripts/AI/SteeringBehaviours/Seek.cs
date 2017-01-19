using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Seek : AgentBehaviour
    {

        public override Steering GetSteering()
        {
            return GetSteering(Target.transform.position);
        }
        public Steering GetSteering(Vector3 targetPosition)
        {
            Steering steering = new Steering();
            if (Agent is Agent2D)
            {
                steering.linear = (Vector2)targetPosition - (Vector2)_transform.position;
            }
            else
            {
                steering.linear = targetPosition - _transform.position;
            }
            steering.linear.Normalize();
            steering.linear = steering.linear * Agent.GetMaxAccel();
            return steering;
        }
        public static Steering GetSteering(Vector3 targetPosition, IAgent agent)
        {
            Steering steering = new Steering();
            if (agent is Agent2D)
            {
                steering.linear = (Vector2)targetPosition - (Vector2)agent.GetTransform().position;
            }
            else
            {
                steering.linear = targetPosition - agent.GetTransform().position;
            }
            steering.linear.Normalize();
            steering.linear = steering.linear * agent.GetMaxAccel();
            return steering;
        }
    }
}
