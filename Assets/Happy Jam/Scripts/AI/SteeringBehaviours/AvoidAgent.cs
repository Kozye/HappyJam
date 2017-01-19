using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class AvoidAgent : AgentBehaviour
    {
        public float CollisionRadius = 0.4f;
        [Header("Targets List")]
        public bool UseTags = false;
        public string AgentTag = "Agent";
        [SerializeField]
        private GameObject[] targets;

        void Start()
        {
            if (UseTags)
                targets = GameObject.FindGameObjectsWithTag("Agent");
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            float shortestTime = Mathf.Infinity;
            GameObject firstTarget = null;
            float firstMinSeparation = 0.0f;
            float firstDistance = 0.0f;
            Vector3 firstRelativePos = Vector3.zero;
            Vector3 firstRelativeVel = Vector3.zero;
            foreach (GameObject t in targets)
            {
                Vector3 relativePos;
                IAgent targetAgent = t.GetComponent<IAgent>();
                relativePos = t.transform.position - transform.position;
                Vector3 relativeVel = targetAgent.GetVelocity() - Agent.GetVelocity();
                float relativeSpeed = relativeVel.magnitude;
                float timeToCollision = Vector3.Dot(relativePos, relativeVel);
                timeToCollision /= relativeSpeed * relativeSpeed * -1;
                float distance = relativePos.magnitude;
                float minSeparation = distance - relativeSpeed * timeToCollision;
                if (minSeparation > 2 * CollisionRadius)
                    continue;
                if (timeToCollision > 0.0f && timeToCollision < shortestTime)
                {
                    shortestTime = timeToCollision;
                    firstTarget = t;
                    firstMinSeparation = minSeparation;
                    firstRelativePos = relativePos;
                    firstRelativeVel = relativeVel;
                }
            }
            if (firstTarget == null)
                return steering;
            if (firstMinSeparation <= 0.0f || firstDistance < 2 * CollisionRadius)
                firstRelativePos = firstTarget.transform.position;
            else
                firstRelativePos += firstRelativeVel * shortestTime;
            firstRelativePos.Normalize();
            steering.linear = -firstRelativePos * Agent.GetMaxAccel();
            return steering;
        }
    }
}
