using UnityEngine;

namespace Assets.Scripts.BattleLogic.StateMachine
{
    [CreateAssetMenu()]
    public class AIAgentConfig: ScriptableObject
    {
        public float RepathFrequency = 0.1f;
        public float MaxSpeed = 2.5f;
        public float BulletSpeed = 7f;
        public float StoppingDistance = 2f;
    }
}
