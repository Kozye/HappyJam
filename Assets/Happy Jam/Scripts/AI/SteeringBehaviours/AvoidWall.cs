using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class AvoidWall : Seek
    {
        public float avoidDistance = 1;
        public float lookAhead = 3;
        private Vector3 _targetPos;

        public override void Awake()
        {
            base.Awake();
            targetRequired = false;
            _targetPos = Vector3.zero;
        }
        // alert
        // agent might try to avoid himself or other agent (if he detect it as wall)
        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            Vector3 position = transform.position;
            Vector3 rayVector = Agent.GetVelocity().normalized * lookAhead;
            Vector3 direction = rayVector;
            Collider col = this.GetComponent<Collider>();

            if (Agent is Agent2D)
            {
                RaycastHit2D hit = Physics2D.Raycast(position + direction.normalized, direction, lookAhead);
                if (hit && (col == null || hit.collider != col))
                {
                    position = hit.point + hit.normal * avoidDistance;
                    _targetPos = position;
                    steering = base.GetSteering(_targetPos);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(position, direction, out hit, lookAhead))
                {
                    position = hit.point + hit.normal * avoidDistance;
                    _targetPos = position;
                    steering = base.GetSteering(_targetPos);
                }
            }
            return steering;
        }
    }
}
