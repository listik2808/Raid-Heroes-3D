using Scripts.Army.TypesSoldiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Economics.Cards.Scripts
{
    [System.Serializable]
    public class Cards
    {
        //public readonly Dictionary<string, int[]> HeroCards = new Dictionary<string, int[]>();
        public readonly Dictionary<Card, int[]> HeroCards = new Dictionary<Card, int[]>();
        public List<Soldier> SimpleCardSoldier = new List<Soldier>();
        public List<Soldier> RareCardSoldier = new List<Soldier>();
        public List<Soldier> EpicCardSoldier = new List<Soldier>();

        public bool Add(Card card, CardType type, int count)
        {
            if (count < 0) return false;

            if (!HeroCards.ContainsKey(card))
                HeroCards.Add(card, new int[3]);

            HeroCards[card][(int)type] += count;

            return true;
        }

        public void Reset()
        {
            foreach (var card in HeroCards.Keys)
            {
                HeroCards[card] = new int[3];
            }
        }

        public int? GetCount(Card card, CardType type)
        {
            if (!HeroCards.ContainsKey(card)) return null;

            return HeroCards[card][(int)type];
        }

        public void GetType(Soldier soldier)
        {
            CardType type  = soldier.DataSoldier.Type;

            if(type == CardType.Simple)
            {
                bool result = SearchSimilarSoldiers(soldier, SimpleCardSoldier);
                TryAddSoldierCard(result, soldier,SimpleCardSoldier);
            }
            else if(type == CardType.Rare)
            {
                bool result = SearchSimilarSoldiers(soldier, RareCardSoldier);
                TryAddSoldierCard(result, soldier, RareCardSoldier);
            }
            else if(type == CardType.Epic)
            {
                bool result = SearchSimilarSoldiers(soldier, EpicCardSoldier);
                TryAddSoldierCard(result, soldier, EpicCardSoldier);
            }
        }

        private bool SearchSimilarSoldiers(Soldier soldier, List<Soldier> typecard)
        {
            foreach (Soldier soldierCard in typecard)
            {
                if (soldierCard.HeroTypeId == soldier.HeroTypeId)
                {
                    return true;
                }
            }
            return false;
        }

        private void TryAddSoldierCard(bool value, Soldier soldier, List<Soldier> typeCardSoldier)
        {
            if (value == false)
                typeCardSoldier.Add(soldier);
        }
    }
}
