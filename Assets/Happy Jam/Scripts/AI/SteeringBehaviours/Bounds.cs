using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Bounds : Seek
    {
        public float avoidDistance = 1;
        public float lookAhead = 3;
        public Vector3 DownLeftBoundary;
        public Vector3 TopRightBoundary;

        public override void Awake()
        {
            base.Awake();
            Target = new GameObject();
        }
        void OnDrawGizmosSelected()
        {
            /*if (Bounderies != null && Bounderies.Length > 0)
                for (int i = 0; i < Bounderies.Length - 1; i++)
                {
                    Gizmos.DrawLine(Bounderies[i], Bounderies[i + 1]);
                }*/

            Vector3 tlBoundary = new Vector3(TopRightBoundary.x, DownLeftBoundary.y, TopRightBoundary.z);
            Vector3 drBoundary = new Vector3(DownLeftBoundary.x, TopRightBoundary.y, TopRightBoundary.z);
            Gizmos.DrawLine(DownLeftBoundary, drBoundary);
            Gizmos.DrawLine(drBoundary, TopRightBoundary);
            Gizmos.DrawLine(TopRightBoundary, tlBoundary);
            Gizmos.DrawLine(tlBoundary, DownLeftBoundary);

        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            Vector3 direction = Agent.GetVelocity().normalized * lookAhead;
            Vector3 position = _transform.position + direction;

            bool outOfBounds = false;
            if (position.x >= TopRightBoundary.x || position.x <= DownLeftBoundary.x)
            {
                outOfBounds = true;
            }
            else if (position.y >= TopRightBoundary.y || position.y <= DownLeftBoundary.y)
            {
                outOfBounds = true;
            }
            if (outOfBounds)
            {
                position = position * -1 * avoidDistance;
                Target.transform.position = position;
                steering = base.GetSteering();
                float orientation;
                direction *= -1;
                if (Agent is Agent2D)
                    orientation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                else
                    orientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                steering.angular = orientation;
                Debug.Log("Out of bounds");
            }

            /* if (Physics.Raycast(position, direction, out hit, lookAhead))
             {
                 position = hit.point + hit.normal * avoidDistance;
                 Target.transform.position = position;
                 steering = base.GetSteering();
             }*/
            return steering;
        }


    }
}
