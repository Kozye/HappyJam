using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Align")]
    [Help("Steering Behaviour Alling that alligns forever that requires Agent component")]
    public class Allign : BaseSteeringBehaviour
    {
        [InParam("target")]
        [Help("Target position where the game object will be moved")]
        public GameObject target;


        [InParam("targetRadius")]
        public float targetRadius;
        [InParam("slowRadius")]
        public float slowRadius;
        [InParam("timeToTarget", DefaultValue = 0.1f)]
        public float timeToTarget = 0.1f;

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
            IAgent targetAgent = target.GetComponent<IAgent>();
            if (targetAgent != null)
                return GetSteering(targetAgent.GetOrientation(), this.agent);
            return null;
        }
        public Steering GetSteering(float targetOrientation, IAgent agent)
        {
            return AI.Allign.GetSteering(targetOrientation, agent, targetRadius, slowRadius, timeToTarget);
        }
    }
}
