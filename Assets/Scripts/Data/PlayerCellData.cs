using Scripts.Army.TypesSoldiers;
using Scripts.BattleTactics;
using Scripts.StaticData;
using System;
using System.Collections.Generic;

namespace Scripts.Data
{
    [Serializable]
    public class PlayerCellData
    {
        public List<HeroTypeId> FieldSoldiers = new List<HeroTypeId>();
        public List<int>Id = new List<int>();
    }
}