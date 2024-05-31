using Scripts.Army.TypesSoldiers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scripts.Data.TypeHeroSoldier
{
    [Serializable]
    public class AllHerosType
    {
        public List<DataLevelSkill> ListTypsHeros = new List<DataLevelSkill>();

        public void AddType(DataLevelSkill data)
        {
            ListTypsHeros.Add(data);
        }

        public int GetCountCardsHero(Soldier soldier)
        {
            foreach (var item in ListTypsHeros)
            {
                if(item.TypeId == soldier.HeroTypeId)
                {
                    return item.CurrentCountCard;
                }
            }
            return 0;
        }
    }
}
