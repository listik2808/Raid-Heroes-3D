using Scripts.Army.TypesSoldiers;
using Scripts.StaticData;
using System;
using System.Collections.Generic;

namespace Scripts.Data
{
    [Serializable]
    public class SquadPlayer
    {
        public List<Soldier> SoldiersPlayer = new List<Soldier>();
        public List<HeroTypeId> HeroTypeIds = new List<HeroTypeId>();
    }
}