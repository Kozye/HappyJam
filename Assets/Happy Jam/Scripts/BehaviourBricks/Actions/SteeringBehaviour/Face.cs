using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Face")]
    [Help("Steering Behaviour Face that alligns forever that requires Agent component")]
    public class Face : Allign
    {
        protected float _targetOrientation = 0;

        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Vector3 direction = target.transform.position - _transform.position;

            if (agent is Agent2D)
                Debug.DrawRay(_transform.position, direction, Color.gray, 2f);
            else
                Debug.DrawRay(_transform.position + Vector3.up, direction, Color.gray, 2f);

            if (direction.magnitude > 0.0f)
            {
                float targetOrientation = 0;

                if (agent is Agent2D)
                    targetOrientation = Mathf.Atan2(direction.y, direction.x);
                else
                    targetOrientation = Mathf.Atan2(direction.x, direction.z);

                targetOrientation *= Mathf.Rad2Deg;
                _targetOrientation = targetOrientation;
            }
            return base.GetSteering(_targetOrientation, agent);
        }
    }
}
