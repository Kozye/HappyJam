using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Face : Allign
    {
        protected float _targetOrientation = 0;

        public override Steering GetSteering()
        {
            Vector3 direction = Target.transform.position - _transform.position;

            if (Agent is Agent2D)
                Debug.DrawRay(transform.position, direction, Color.gray, 2f);
            else
                Debug.DrawRay(transform.position + Vector3.up, direction, Color.gray, 2f);

            if (direction.magnitude > 0.0f)
            {
                float targetOrientation = 0;

                if (Agent is Agent2D)
                    targetOrientation = Mathf.Atan2(direction.y, direction.x);
                else
                    targetOrientation = Mathf.Atan2(direction.x, direction.z);

                targetOrientation *= Mathf.Rad2Deg;
                _targetOrientation = targetOrientation;
            }
            return base.GetSteering(_targetOrientation, Agent);
        }
    }
}
