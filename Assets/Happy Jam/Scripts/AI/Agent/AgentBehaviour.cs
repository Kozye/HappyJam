using System.Collections.Generic;
using UnityEngine;
namespace AI
{
    public class AgentBehaviour : MonoBehaviour
    {

        public GameObject Target;
        protected IAgent Agent;
        public float Weight = 1.0f;
        public int Priority = 1;
        protected bool targetRequired = true;
        protected Transform _transform;
        // Use this for initialization
        public virtual void Awake()
        {
            Agent = gameObject.GetComponent<IAgent>();
            _transform = transform;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (Target != null || !targetRequired)
            {
                Agent.SetSteering(GetSteering(), Priority);
            }
            else if (targetRequired)
            {
                Debug.LogError("AgentBehaviour: Target is null");
            }
        }
        public virtual Steering GetSteering()
        {
            return new Steering();
        }
        public static float MapToRange(float rotation)
        {
            rotation %= 360.0f;
            if (Mathf.Abs(rotation) > 180.0f)
            {
                if (rotation < 0.0f)
                    rotation += 360.0f;
                else
                    rotation -= 360.0f;
            }
            return rotation;
        }

        public static Vector3 GetOriAsVec(float orientation)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
            vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
            return vector.normalized;
        }
        public static Vector3 GetOriAsVec2D(float orientation)
        {
            Vector3 vector = Vector3.zero;
            vector.y = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
            vector.x = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
            return vector.normalized;
        }

        public static Vector3 GetOriAsVector(float orientation)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
            vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
            return vector.normalized;
        }
    }
}

