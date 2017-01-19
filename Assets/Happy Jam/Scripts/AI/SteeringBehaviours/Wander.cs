using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Wander : Face
    {
        public float Offset = 5;
        public float Radius = 3;
        public float Rate = 60;

        public override void Awake()
        {
            Target = new GameObject();
            Target.transform.position = transform.position;
            base.Awake();
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            float wanderOrientation = Random.Range(-1.0f, 1.0f) * Rate;
            float targetOrientation = wanderOrientation + Agent.GetOrientation();
            if (Agent is Agent2D)
            {
                Vector3 orientationVec = GetOriAsVec2D(Agent.GetOrientation());
                Vector3 targetPosition = (Offset * orientationVec) + transform.position;
                targetPosition = targetPosition + (GetOriAsVec2D(targetOrientation) * Radius);
                Target.transform.position = targetPosition;
            }
            else
            {
                Vector3 orientationVec = GetOriAsVec(Agent.GetOrientation());
                Vector3 targetPosition = (Offset * orientationVec) + transform.position;
                targetPosition = targetPosition + (GetOriAsVec(targetOrientation) * Radius);
                Target.transform.position = targetPosition;
            }
            steering = base.GetSteering();
            steering.linear = Target.transform.position - transform.position;
            steering.linear.Normalize();
            steering.linear *= Agent.GetMaxAccel();
            return steering;
        }
    }
}
