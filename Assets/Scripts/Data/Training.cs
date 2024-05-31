using Scripts.StaticData;
using System;

namespace Scripts.Data
{
    [Serializable]
    public class Training
    {
        public bool Tutor = false;
        public bool UpLevelSpecSkill = false;
        public HeroTypeId HeroType;
        public int CountCard = 0;
        public bool Finish = false;
    }
}