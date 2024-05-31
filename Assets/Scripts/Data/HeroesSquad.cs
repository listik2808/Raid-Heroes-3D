using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scripts.Data
{
    [Serializable]
    public class HeroesSquad
    {
        public List<Soldier> SoldierHerosSquad = new List<Soldier>();
        public List<HeroTypeId> HeroTypeIds = new List<HeroTypeId>();

        public void SetHerosSquadCastle(Soldier soldier)
        {
           if(CheckingCardAvailability(soldier) == false)
           {
                HeroTypeIds.Add(soldier.HeroTypeId);
                SoldierHerosSquad.Add(soldier);
           }
        }

        private bool CheckingCardAvailability(Soldier soldier)
        {
            foreach (var item in HeroTypeIds)
            {
                if (item == soldier.HeroTypeId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
