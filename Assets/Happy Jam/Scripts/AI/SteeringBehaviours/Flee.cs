using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Flee : AgentBehaviour
    {

        public override Steering GetSteering()
        {
            return GetSteering(Target.transform.position);
        }
        public Steering GetSteering(Vector3 targetPosition)
        {
            return GetSteering(targetPosition, Agent);
        }
        public static Steering GetSteering(Vector3 targetPosition, IAgent agent)
        {
            Steering steering = new Steering();
            if (agent is Agent2D)
            {
                steering.linear = (Vector2)agent.GetTransform().position - (Vector2)targetPosition;
            }
            else
            {
                steering.linear = agent.GetTransform().position - targetPosition;
            }
            steering.linear.Normalize();
            steering.linear = steering.linear * agent.GetMaxAccel();
            return steering;
        }
    }
}
