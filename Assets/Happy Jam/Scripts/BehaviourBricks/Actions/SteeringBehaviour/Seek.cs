using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Seek")]
    [Help("Steering Behaviour Seek that requires Agent component")]
    public class Seek : BaseSteeringBehaviour
    {
        [InParam("target obj")]
        [Help("Target position from which agent will flee")]
        public GameObject target;

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
            if (target)
                return GetSteering(target.transform.position);
            return null;
        }
        public Steering GetSteering(Vector3 position)
        {
            return AI.Seek.GetSteering(position, agent);
        }
    }
}
