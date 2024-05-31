using System;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.CharacteristicsSoldier
{
    public abstract class LevelDataSkill : MonoBehaviour
    {
        protected float CurrentStepSkillValue = 0;
        protected float ImprovementStep = 1f;
        protected float MaxStep = 10;

        public float CurrentStepSkill => CurrentStepSkillValue;
        public float MaxStepValue => MaxStep;

        public event Action UpdateStepSkill;

        public void ResetStepLevelSkill()
        {
            CurrentStepSkillValue = 0;
            UpdateStepSkill?.Invoke();
        }

        public void AddStep()
        {
            CurrentStepSkillValue += ImprovementStep;
            UpdateStepSkill?.Invoke();
        }

        public void LoadStepCurrent(float step)
        {
            CurrentStepSkillValue = step;
        }

        public int AddStepEnemy()
        {
            CurrentStepSkillValue += ImprovementStep;
            if(CurrentStepSkillValue == 11)
            {
                CurrentStepSkillValue = 0;
                return 1;
            }
            return 0;
        }

        public void SetStepSkill(int value)
        {
            if(value < MaxStep)
            {
                CurrentStepSkillValue = value;
            }
            else
            {
                value = (int)MaxStep;
                CurrentStepSkillValue = value;
            }
        }
    }
}
