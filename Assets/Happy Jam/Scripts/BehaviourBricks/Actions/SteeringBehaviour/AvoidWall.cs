using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{
    [Action("Steering Behaviour/Avoid Wall")]
    [Help("Steering Behaviour Pursue that requires Agent component. Target should be null")]
    public class AvoidWall : Seek
    {
        [InParam("avoidDistance", DefaultValue = 1f)]
        public float avoidDistance;
        [InParam("lookAhead", DefaultValue = 3f)]
        public float lookAhead;

        protected Vector3 _targetPosition;
        protected IAgent _targetAgent;

        public override void OnStart()
        {
            base.OnStart();
            if (target == null)
                target = new GameObject();
        }

        public override TaskStatus OnUpdate()
        {
            if (_transform)
                SetSteering();
            return TaskStatus.RUNNING;
        }
        // alert
        // agent might try to avoid himself or other agent (if he detect it as wall)
        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            Vector3 position = _transform.position;
            Vector3 rayVector = agent.GetVelocity().normalized * lookAhead;
            Vector3 direction = rayVector;
            Collider col = _transform.GetComponent<Collider>();

            if (agent is Agent2D)
            {
                RaycastHit2D hit = Physics2D.Raycast(position + direction.normalized, direction, lookAhead);
                if (hit && (col == null || hit.collider != col))
                {
                    position = hit.point + hit.normal * avoidDistance;
                    target.transform.position = position;
                    steering = base.GetSteering();
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(position, direction, out hit, lookAhead))
                {
                    position = hit.point + hit.normal * avoidDistance;
                    target.transform.position = position;
                    steering = base.GetSteering();
                }
            }
            return steering;
        }

    }
}
